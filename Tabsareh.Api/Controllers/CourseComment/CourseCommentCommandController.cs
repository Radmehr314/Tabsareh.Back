using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.CourseComments;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.CourseComment
{
    [Authorize(Policy = "manage_comments")]
    public class CourseCommentCommandController : BaseCommandController
    {
        public CourseCommentCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("ApproveCourseComment")]
        public async Task<IActionResult> Approve([FromBody] ApproveCourseCommentCommand command)
        {
            await Bus.Dispatch(command);
            return Ok();
        }

        [HttpPost("RejectCourseComment")]
        public async Task<IActionResult> Reject([FromBody] RejectCourseCommentCommand command)
        {
            await Bus.Dispatch(command);
            return Ok();
        }
    }
}
