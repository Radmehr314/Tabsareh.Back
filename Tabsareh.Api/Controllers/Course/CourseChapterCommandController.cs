using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.Courses;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Course
{
    [Authorize(Policy = "manage_course_chapters")]
    public class CourseChapterCommandController : BaseCommandController
    {
        public CourseChapterCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddCourseChapter")]
        public async Task<ActionResult<CommandResult>> AddCourseChapter([FromBody] AddCourseChapterCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UpdateCourseChapter")]
        public async Task<ActionResult<CommandResult>> UpdateCourseChapter([FromBody] UpdateCourseChapterCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpDelete("DeleteCourseChapter")]
        public async Task<ActionResult<CommandResult>> DeleteCourseChapter([FromBody] DeleteCourseChapterCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }
    }
}
