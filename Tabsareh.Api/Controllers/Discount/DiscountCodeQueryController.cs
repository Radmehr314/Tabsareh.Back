using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.Discounts;
using Tabsareh.Application.Contracts.QueryResult.Discounts;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Discount
{
    [Authorize(Policy = "manage_discounts")]
    public class DiscountCodeQueryController : BaseQueryController
    {
        public DiscountCodeQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("DiscountCodes")]
        public async Task<ActionResult<GetDiscountCodesPagedQueryResult>> GetDiscountCodes([FromQuery] GetDiscountCodesPagedQuery query)
            => Ok(await Bus.Dispatch<GetDiscountCodesPagedQuery, GetDiscountCodesPagedQueryResult>(query));

        [HttpGet("GetDiscountCode")]
        public async Task<ActionResult<DiscountCodeItemResult>> GetDiscountCode([FromQuery] GetDiscountCodeByIdQuery query)
            => Ok(await Bus.Dispatch<GetDiscountCodeByIdQuery, DiscountCodeItemResult>(query));
    }
}
