using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.Cart;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Cart
{
    [Authorize(Roles = "user")]
    public class CartCommandController : BaseCommandController
    {
        public CartCommandController(ICommandBus bus) : base(bus) { }

        [HttpPost("AddToCart")]
        public async Task<ActionResult<CommandResult>> AddToCart([FromBody] AddToCartCommand command)
            => Ok(await Bus.Dispatch(command));

        [HttpDelete("RemoveFromCart")]
        public async Task<ActionResult<CommandResult>> RemoveFromCart([FromBody] RemoveFromCartCommand command)
            => Ok(await Bus.Dispatch(command));

        [HttpDelete("ClearCart")]
        public async Task<ActionResult<CommandResult>> ClearCart()
            => Ok(await Bus.Dispatch(new ClearCartCommand()));

        [HttpPost("CreateOrderFromCart")]
        public async Task<ActionResult<CommandResult>> CreateOrderFromCart([FromBody] CreateOrderFromCartCommand command)
            => Ok(await Bus.Dispatch(command));
    }
}
