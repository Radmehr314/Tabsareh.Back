using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.Discounts;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Discount
{
    [Authorize(Policy = "manage_discounts")]
    public class DiscountCodeCommandController : BaseCommandController
    {
        public DiscountCodeCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddDiscountCode")]
        public async Task<ActionResult<CommandResult>> AddDiscountCode([FromBody] AddDiscountCodeCommand command)
            => Ok(await Bus.Dispatch(command));

        [HttpPut("UpdateDiscountCode")]
        public async Task<ActionResult<CommandResult>> UpdateDiscountCode([FromBody] UpdateDiscountCodeCommand command)
            => Ok(await Bus.Dispatch(command));

        [HttpDelete("DeleteDiscountCode")]
        public async Task<ActionResult<CommandResult>> DeleteDiscountCode([FromBody] DeleteDiscountCodeCommand command)
            => Ok(await Bus.Dispatch(command));
    }
}
