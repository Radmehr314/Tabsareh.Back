using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.CourseComments;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.CourseComment
{
    [AllowAnonymous]
    public class SiteCourseCommentCommandController : BaseCommandController
    {
        public SiteCourseCommentCommandController(ICommandBus bus) : base(bus) { }

        /// <summary>
        /// ارسال کامنت جدید برای یک دوره از سایت اصلی.
        /// کامنت پس از ارسال در انتظار تأیید ادمین قرار می‌گیرد.
        /// </summary>
        [HttpPost("SubmitCourseComment")]
        public async Task<IActionResult> Submit([FromBody] AddCourseCommentCommand command)
        {
            await Bus.Dispatch(command);
            return Ok(new { message = "کامنت شما ثبت شد و پس از تأیید ادمین نمایش داده خواهد شد." });
        }
    }
}
