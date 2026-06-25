using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.Courses;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Course
{
    [Authorize(Policy = "manage_courses")]
    public class CourseCommandController : BaseCommandController
    {
        public CourseCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddCourse")]
        public async Task<ActionResult<CommandResult>> AddCourse([FromBody] AddCourseCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UpdateCourse")]
        public async Task<ActionResult<CommandResult>> UpdateCourse([FromBody] UpdateCourseCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpDelete("DeleteCourse")]
        public async Task<ActionResult<CommandResult>> DeleteCourse([FromBody] DeleteCourseCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }
    }
}
