using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.Orders;
using Tabsareh.Application.Contracts.QueryResult.Orders;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Order
{
    [Authorize]
    public class OrderQueryController : BaseQueryController
    {
        public OrderQueryController(IQueryBus bus) : base(bus) { }

        [Authorize(Policy = "manage_orders")]
        [HttpGet("Orders")]
        public async Task<ActionResult<GetOrdersPagedQueryResult>> GetOrders([FromQuery] GetOrdersPagedQuery query)
            => Ok(await Bus.Dispatch<GetOrdersPagedQuery, GetOrdersPagedQueryResult>(query));

        [Authorize(Policy = "manage_card_to_card")]
        [HttpGet("PendingCardToCardOrders")]
        public async Task<ActionResult<GetOrdersPagedQueryResult>> GetPendingCardToCardOrders([FromQuery] GetPendingCardToCardOrdersQuery query)
            => Ok(await Bus.Dispatch<GetPendingCardToCardOrdersQuery, GetOrdersPagedQueryResult>(query));

        [HttpGet("MyOrders")]
        public async Task<ActionResult<List<OrderItemResult>>> GetMyOrders()
            => Ok(await Bus.Dispatch<GetMyOrdersQuery, List<OrderItemResult>>(new GetMyOrdersQuery()));

        [HttpPost("PreviewOrderInvoice")]
        public async Task<ActionResult<OrderInvoiceResult>> PreviewOrderInvoice([FromBody] PreviewOrderInvoiceQuery query)
            => Ok(await Bus.Dispatch<PreviewOrderInvoiceQuery, OrderInvoiceResult>(query));
    }
}
