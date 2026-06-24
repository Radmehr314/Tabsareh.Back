using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tabsareh.Api.Controllers.File
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private static readonly HashSet<string> AllowedExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

        private const long MaxImageSize = 5 * 1024 * 1024; // 5 MB

        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// آپلود تصویر. فایل را ذخیره می‌کند و مسیر عمومی آن را برمی‌گرداند
        /// تا در کامندهایی مثل AddTeacher داخل فیلد ProfileImage استفاده شود.
        /// </summary>
        [HttpPost("Image")]
        [RequestSizeLimit(MaxImageSize)]
        public async Task<ActionResult<UploadResult>> UploadImage(IFormFile file, [FromQuery] string folder = "teachers")
        {
            if (file is null || file.Length == 0)
                return BadRequest(new { ErrorCode = "ERR_400", ErrorMessage = "فایلی ارسال نشده است." });

            if (file.Length > MaxImageSize)
                return BadRequest(new { ErrorCode = "ERR_400", ErrorMessage = "حجم تصویر بیش از حد مجاز (۵ مگابایت) است." });

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(ext) || !AllowedExtensions.Contains(ext))
                return BadRequest(new { ErrorCode = "ERR_400", ErrorMessage = "فرمت تصویر مجاز نیست. (jpg, jpeg, png, webp, gif)" });

            // جلوگیری از path traversal در نام پوشه
            var safeFolder = string.Concat((folder ?? "teachers").Where(char.IsLetterOrDigit));
            if (string.IsNullOrWhiteSpace(safeFolder)) safeFolder = "teachers";

            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var targetDir = Path.Combine(webRoot, "uploads", safeFolder);
            Directory.CreateDirectory(targetDir);

            var fileName = $"{Guid.NewGuid():N}{ext.ToLowerInvariant()}";
            var fullPath = Path.Combine(targetDir, fileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"/uploads/{safeFolder}/{fileName}";
            return Ok(new UploadResult { Url = url, FileName = fileName });
        }

        public class UploadResult
        {
            public string Url { get; set; }
            public string FileName { get; set; }
        }
    }
}
