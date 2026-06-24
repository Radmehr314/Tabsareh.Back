using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;
using Tabsareh.Application.Contracts.Commands.Teachers;

namespace Tabsareh.Api.Controllers.Teacher
{
    [Authorize(Policy = "manage_teachers")]
    public class TeacherCommandController : BaseCommandController
    {
        public TeacherCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddTeacher")]
        public async Task<ActionResult<CommandResult>> AddTeacher([FromBody] AddTeacherCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UpdateTeacher")]
        public async Task<ActionResult<CommandResult>> UpdateTeacher([FromBody] UpdateTeacherCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpDelete("DeleteTeacher")]
        public async Task<ActionResult<CommandResult>> DeleteTeacher([FromBody] DeleteTeacherCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }
    }
}
