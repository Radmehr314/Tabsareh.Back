using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;
using Tabsareh.Application.Contracts.Commands.Roles;

namespace Tabsareh.Api.Controllers.Role
{
    [Authorize(Policy = "manage_roles")]
    public class RoleCommandController : BaseCommandController
    {
        public RoleCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddRole")]
        public async Task<ActionResult<CommandResult>> AddRole([FromBody] AddRoleCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UpdateRole")]
        public async Task<ActionResult<CommandResult>> UpdateRole([FromBody] UpdateRoleCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpDelete("DeleteRole")]
        public async Task<ActionResult<CommandResult>> DeleteRole([FromBody] DeleteRoleCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }
    }
}
