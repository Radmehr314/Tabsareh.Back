using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.DynamicPages;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.DynamicPage
{
    [Authorize(Policy = "manage_dynamic_pages")]
    public class DynamicPageCommandController : BaseCommandController
    {
        public DynamicPageCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddDynamicPage")]
        public async Task<ActionResult<CommandResult>> AddDynamicPage([FromBody] AddDynamicPageCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpPut("UpdateDynamicPage")]
        public async Task<ActionResult<CommandResult>> UpdateDynamicPage([FromBody] UpdateDynamicPageCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }

        [HttpDelete("DeleteDynamicPage")]
        public async Task<ActionResult<CommandResult>> DeleteDynamicPage([FromBody] DeleteDynamicPageCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }
    }
}
