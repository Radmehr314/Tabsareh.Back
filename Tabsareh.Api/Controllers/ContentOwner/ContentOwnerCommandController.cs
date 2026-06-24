using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;
using Tabsareh.Application.Contracts.Commands.ContentOwners;

namespace Tabsareh.Api.Controllers.ContentOwner
{
    [Authorize(Policy = "manage_content_owners")]
    public class ContentOwnerCommandController : BaseCommandController
    {
        public ContentOwnerCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddContentOwner")]
        public async Task<ActionResult<CommandResult>> AddContentOwner([FromBody] AddContentOwnerCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UpdateContentOwner")]
        public async Task<ActionResult<CommandResult>> UpdateContentOwner([FromBody] UpdateContentOwnerCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("BanContentOwner")]
        public async Task<ActionResult<CommandResult>> BanContentOwner([FromBody] BanContentOwnerCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UnbanContentOwner")]
        public async Task<ActionResult<CommandResult>> UnBanContentOwner([FromBody] UnBanContentOwnerCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpDelete("DeleteContentOwner")]
        public async Task<ActionResult<CommandResult>> DeleteContentOwner([FromBody] DeleteContentOwnerCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }
    }
}
