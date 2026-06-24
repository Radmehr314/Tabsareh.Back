using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.Users;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.User
{
    [Authorize(Policy = "manage_users")]
    public class UserCommandController : BaseCommandController
    {
        public UserCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddUser")]
        public async Task<ActionResult<CommandResult>> AddUser([FromBody] AddUserCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UpdateUser")]
        public async Task<ActionResult<CommandResult>> UpdateUser([FromBody] UpdateUserCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<CommandResult>> DeleteUser([FromBody] DeleteUserCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }
    }
}
