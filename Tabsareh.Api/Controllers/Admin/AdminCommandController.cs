using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;
using Tabsareh.Application.Contracts.Commands.Admins;

namespace Tabsareh.Api.Controllers.Admin
{
    [Authorize(Policy = "manage_admins")]
    public class AdminCommandController : BaseCommandController
    {
        public AdminCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddAdmin")]
        public async Task<ActionResult<CommandResult>> AddAdmin([FromBody] AddAdminCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UpdateAdmin")]
        public async Task<ActionResult<CommandResult>> UpdateAdmin([FromBody] UpdateAdminCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("BanAdmin")]
        public async Task<ActionResult<CommandResult>> BanAdmin([FromBody] BanAdminCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UnbanAdmin")]
        public async Task<ActionResult<CommandResult>> UnBanAdmin([FromBody] UnBanAdminCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpDelete("DeleteAdmin")]
        public async Task<ActionResult<CommandResult>> DeleteAdmin([FromBody] DeleteAdminCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }
    }
}
