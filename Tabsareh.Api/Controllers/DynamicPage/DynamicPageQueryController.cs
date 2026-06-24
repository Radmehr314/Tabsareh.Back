using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.DynamicPage;
using Tabsareh.Application.Contracts.QueryResult.DynamicPage;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.DynamicPage
{
    [Authorize(Policy = "manage_dynamic_pages")]
    public class DynamicPageQueryController : BaseQueryController
    {
        public DynamicPageQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("DynamicPages")]
        public async Task<ActionResult<GetDynamicPagesPagedQueryResult>> GetDynamicPagesPaged([FromQuery] GetDynamicPagesPagedQuery query)
        {
            return Ok(await Bus.Dispatch<GetDynamicPagesPagedQuery, GetDynamicPagesPagedQueryResult>(query));
        }

        [HttpGet("AllDynamicPages")]
        public async Task<ActionResult<List<DynamicPageItemResult>>> GetAllDynamicPages()
        {
            return Ok(await Bus.Dispatch<GetAllDynamicPagesQuery, List<DynamicPageItemResult>>(new GetAllDynamicPagesQuery()));
        }

        [HttpGet("GetDynamicPage")]
        public async Task<ActionResult<DynamicPageItemResult>> GetDynamicPage([FromQuery] GetDynamicPageByIdQuery query)
        {
            return Ok(await Bus.Dispatch<GetDynamicPageByIdQuery, DynamicPageItemResult>(query));
        }

        [HttpGet("GetDynamicPageBySlug")]
        public async Task<ActionResult<DynamicPageItemResult>> GetDynamicPageBySlug([FromQuery] GetDynamicPageBySlugQuery query)
        {
            return Ok(await Bus.Dispatch<GetDynamicPageBySlugQuery, DynamicPageItemResult>(query));
        }
    }
}
