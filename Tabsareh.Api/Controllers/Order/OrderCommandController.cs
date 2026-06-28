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

        [Authorize(Policy = "manage_card_to_card")]
        [HttpPost("ApproveCardToCardOrder")]
        public async Task<ActionResult<CommandResult>> ApproveCardToCardOrder([FromBody] ApproveCardToCardOrderCommand command)
            => Ok(await Bus.Dispatch(command));

        [Authorize(Policy = "manage_card_to_card")]
        [HttpPost("RejectCardToCardOrder")]
        public async Task<ActionResult<CommandResult>> RejectCardToCardOrder([FromBody] RejectCardToCardOrderCommand command)
            => Ok(await Bus.Dispatch(command));

        /// <summary>
        /// فراخوانی توسط فرانت‌اند پس از بازگشت کاربر از صفحه پرداخت سامان
        /// نیازی به احراز هویت ندارد چون سامان کاربر را redirect می‌کند
        /// </summary>
        [AllowAnonymous]
        [HttpPost("VerifyGatewayPayment")]
        public async Task<ActionResult<CommandResult>> VerifyGatewayPayment([FromBody] VerifyGatewayPaymentCommand command)
            => Ok(await Bus.Dispatch(command));
    }
}
