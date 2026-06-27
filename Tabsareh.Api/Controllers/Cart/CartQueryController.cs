using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.Cart;
using Tabsareh.Application.Contracts.QueryResult.Cart;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Cart
{
    [Authorize(Roles = "user")]
    public class CartQueryController : BaseQueryController
    {
        public CartQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("MyCart")]
        public async Task<ActionResult<CartResult>> GetMyCart()
            => Ok(await Bus.Dispatch<GetMyCartQuery, CartResult>(new GetMyCartQuery()));
    }
}
