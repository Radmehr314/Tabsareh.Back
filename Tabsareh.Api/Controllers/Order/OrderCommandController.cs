using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.Orders;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Order
{
    [Authorize]
    public class OrderCommandController : BaseCommandController
    {
        public OrderCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<CommandResult>> CreateOrder([FromBody] CreateOrderCommand command)
            => Ok(await Bus.Dispatch(command));

        [Authorize(Policy = "manage_orders")]
        [HttpPost("ApproveCardToCardOrder")]
        public async Task<ActionResult<CommandResult>> ApproveCardToCardOrder([FromBody] ApproveCardToCardOrderCommand command)
            => Ok(await Bus.Dispatch(command));

        [Authorize(Policy = "manage_orders")]
        [HttpPost("RejectCardToCardOrder")]
        public async Task<ActionResult<CommandResult>> RejectCardToCardOrder([FromBody] RejectCardToCardOrderCommand command)
            => Ok(await Bus.Dispatch(command));
    }
}
