using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Tabsareh.Api.Controllers.File
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private static readonly HashSet<string> AllowedImageExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private static readonly HashSet<string> AllowedDocumentExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".pdf" };
        private static readonly HashSet<string> AllowedVideoExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".mp4", ".mov", ".m4v" };

        private const long MaxImageSize = 5 * 1024 * 1024;
        private const long MaxDocumentSize = 25 * 1024 * 1024;
        private const long MaxVideoSize = 500 * 1024 * 1024;

        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("Image")]
        [RequestSizeLimit(MaxImageSize)]
        public async Task<ActionResult<UploadResult>> UploadImage(IFormFile file, [FromQuery] string folder = "teachers")
        {
            var error = ValidateFile(file, MaxImageSize, AllowedImageExtensions, "فرمت تصویر مجاز نیست.");
            if (error != null) return error;

            var ext = Path.GetExtension(file.FileName);
            var result = await SaveFile(file, ext, folder, "teachers");
            return Ok(new UploadResult { Url = result.Url, FileName = result.FileName });
        }

        [HttpPost("File")]
        [RequestSizeLimit(MaxDocumentSize)]
        public async Task<ActionResult<UploadResult>> UploadFile(IFormFile file, [FromQuery] string folder = "courses")
        {
            var error = ValidateFile(file, MaxDocumentSize, AllowedDocumentExtensions, "فرمت فایل مجاز نیست. (pdf)");
            if (error != null) return error;

            var ext = Path.GetExtension(file.FileName);
            var result = await SaveFile(file, ext, folder, "courses");
            return Ok(new UploadResult { Url = result.Url, FileName = result.FileName });
        }

        [HttpPost("Video")]
        [RequestSizeLimit(MaxVideoSize)]
        public async Task<ActionResult<VideoUploadResult>> UploadVideo(IFormFile file, [FromQuery] string folder = "courses")
        {
            var error = ValidateFile(file, MaxVideoSize, AllowedVideoExtensions, "فرمت ویدیو مجاز نیست. (mp4, mov, m4v)");
            if (error != null) return error;

            var ext = Path.GetExtension(file.FileName);
            var result = await SaveFile(file, ext, folder, "courses");

            return Ok(new VideoUploadResult
            {
                Url = result.Url,
                FileName = result.FileName,
                Duration = FormatDuration(TryReadMp4Duration(result.FullPath))
            });
        }

        [HttpPost("ExternalVideo")]
        [RequestSizeLimit(MaxVideoSize)]
        public async Task<ActionResult<ExternalVideoUploadResult>> UploadExternalVideo(IFormFile file)
        {
            var error = ValidateFile(file, MaxVideoSize, AllowedVideoExtensions, "فرمت ویدیو مجاز نیست. (mp4, mov, m4v)");
            if (error != null) return error;

            var ext = Path.GetExtension(file.FileName);
            var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}{ext.ToLowerInvariant()}");

            try
            {
                await using (var stream = new FileStream(tempPath, FileMode.Create))
                    await file.CopyToAsync(stream);

                return Ok(new ExternalVideoUploadResult
                {
                    FileName = file.FileName,
                    Duration = FormatDuration(TryReadMp4Duration(tempPath)),
                    ExternalVideoId = null,
                    VideoUrl = null,
                    UploadStatus = "PendingExternalUpload",
                    NotConfigured = true
                });
            }
            finally
            {
                if (System.IO.File.Exists(tempPath))
                    System.IO.File.Delete(tempPath);
            }
        }

        private ActionResult? ValidateFile(IFormFile file, long maxSize, HashSet<string> allowedExtensions, string formatMessage)
        {
            if (file is null || file.Length == 0)
                return BadRequest(new { ErrorCode = "ERR_400", ErrorMessage = "فایلی ارسال نشده است." });

            if (file.Length > maxSize)
                return BadRequest(new { ErrorCode = "ERR_400", ErrorMessage = "حجم فایل بیش از حد مجاز است." });

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(ext) || !allowedExtensions.Contains(ext))
                return BadRequest(new { ErrorCode = "ERR_400", ErrorMessage = formatMessage });

            return null;
        }

        private async Task<(string Url, string FileName, string FullPath)> SaveFile(IFormFile file, string ext, string folder, string fallbackFolder)
        {
            var safeFolder = string.Concat((folder ?? fallbackFolder).Where(char.IsLetterOrDigit));
            if (string.IsNullOrWhiteSpace(safeFolder)) safeFolder = fallbackFolder;

            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var targetDir = Path.Combine(webRoot, "uploads", safeFolder);
            Directory.CreateDirectory(targetDir);

            var fileName = $"{Guid.NewGuid():N}{ext.ToLowerInvariant()}";
            var fullPath = Path.Combine(targetDir, fileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
                await file.CopyToAsync(stream);

            return ($"/uploads/{safeFolder}/{fileName}", fileName, fullPath);
        }

        private static TimeSpan? TryReadMp4Duration(string path)
        {
            try
            {
                using var stream = System.IO.File.OpenRead(path);
                return FindMvhdDuration(stream, stream.Length);
            }
            catch
            {
                return null;
            }
        }

        private static TimeSpan? FindMvhdDuration(Stream stream, long end)
        {
            while (stream.Position + 8 <= end)
            {
                var atomStart = stream.Position;
                var size = ReadUInt32(stream);
                var type = ReadAscii(stream, 4);
                long atomSize = size;

                if (size == 1)
                    atomSize = (long)ReadUInt64(stream);
                if (atomSize < 8 || atomStart + atomSize > end)
                    break;

                if (type == "moov" || type == "trak" || type == "mdia")
                {
                    var nested = FindMvhdDuration(stream, atomStart + atomSize);
                    if (nested.HasValue) return nested;
                }
                else if (type == "mvhd")
                {
                    var version = stream.ReadByte();
                    stream.Position += 3;
                    if (version == 1)
                    {
                        stream.Position += 16;
                        var timescale = ReadUInt32(stream);
                        var duration = ReadUInt64(stream);
                        return timescale == 0 ? null : TimeSpan.FromSeconds(duration / (double)timescale);
                    }

                    stream.Position += 8;
                    var scale = ReadUInt32(stream);
                    var dur = ReadUInt32(stream);
                    return scale == 0 ? null : TimeSpan.FromSeconds(dur / (double)scale);
                }

                stream.Position = atomStart + atomSize;
            }

            return null;
        }

        private static uint ReadUInt32(Stream stream)
        {
            Span<byte> bytes = stackalloc byte[4];
            stream.ReadExactly(bytes);
            return ((uint)bytes[0] << 24) | ((uint)bytes[1] << 16) | ((uint)bytes[2] << 8) | bytes[3];
        }

        private static ulong ReadUInt64(Stream stream)
        {
            Span<byte> bytes = stackalloc byte[8];
            stream.ReadExactly(bytes);
            return ((ulong)bytes[0] << 56) | ((ulong)bytes[1] << 48) | ((ulong)bytes[2] << 40) | ((ulong)bytes[3] << 32) |
                   ((ulong)bytes[4] << 24) | ((ulong)bytes[5] << 16) | ((ulong)bytes[6] << 8) | bytes[7];
        }

        private static string ReadAscii(Stream stream, int length)
        {
            Span<byte> bytes = stackalloc byte[length];
            stream.ReadExactly(bytes);
            return Encoding.ASCII.GetString(bytes);
        }

        private static string FormatDuration(TimeSpan? duration)
        {
            if (!duration.HasValue) return "00:00";
            var value = duration.Value;
            if (value.TotalHours >= 1) return $"{(int)value.TotalHours}:{value.Minutes:00}:{value.Seconds:00}";
            return value.Minutes == 0 ? $"00:{value.Seconds:00}" : $"{value.Minutes}:{value.Seconds:00}";
        }

        public class UploadResult
        {
            public string Url { get; set; }
            public string FileName { get; set; }
        }

        public class VideoUploadResult : UploadResult
        {
            public string Duration { get; set; }
        }

        public class ExternalVideoUploadResult
        {
            public string FileName { get; set; }
            public string Duration { get; set; }
            public string? ExternalVideoId { get; set; }
            public string? VideoUrl { get; set; }
            public string UploadStatus { get; set; }
            public bool NotConfigured { get; set; }
        }
    }
}
