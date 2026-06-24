using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.DynamicPage;
using Tabsareh.Application.Contracts.QueryResult.DynamicPage;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.DynamicPage
{
    [AllowAnonymous]
    public class SiteDynamicPageQueryController : BaseQueryController
    {
        public SiteDynamicPageQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("DynamicPageMenu")]
        public async Task<ActionResult<List<DynamicPageMenuItemResult>>> GetDynamicPageMenu()
        {
            return Ok(await Bus.Dispatch<GetPublishedDynamicPageMenuQuery, List<DynamicPageMenuItemResult>>(new GetPublishedDynamicPageMenuQuery()));
        }

        [HttpGet("DynamicPageBySlug")]
        public async Task<ActionResult<DynamicPageItemResult>> GetDynamicPageBySlug([FromQuery] GetPublishedDynamicPageBySlugQuery query)
        {
            return Ok(await Bus.Dispatch<GetPublishedDynamicPageBySlugQuery, DynamicPageItemResult>(query));
        }
    }
}
