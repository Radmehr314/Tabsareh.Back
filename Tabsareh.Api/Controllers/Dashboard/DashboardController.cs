using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.Dashboard;
using Tabsareh.Application.Contracts.QueryResult.Dashboard;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Dashboard
{
    [Authorize(Roles = "admin")]
    public class DashboardController : BaseQueryController
    {
        public DashboardController(IQueryBus bus) : base(bus) { }

        [HttpGet("DashboardStats")]
        public async Task<ActionResult<DashboardStatsResult>> GetStats()
            => Ok(await Bus.Dispatch<GetDashboardStatsQuery, DashboardStatsResult>(new GetDashboardStatsQuery()));
    }
}
