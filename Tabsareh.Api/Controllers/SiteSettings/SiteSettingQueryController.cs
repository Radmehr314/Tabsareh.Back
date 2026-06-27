using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.SiteSettings;
using Tabsareh.Application.Contracts.QueryResult.SiteSettings;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.SiteSettings
{
    public class SiteSettingQueryController : BaseQueryController
    {
        public SiteSettingQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("Settings")]
        [Authorize(Policy = "manage_courses")]
        public async Task<ActionResult<SiteSettingsResult>> GetSettings()
        {
            return Ok(await Bus.Dispatch<GetSiteSettingsQuery, SiteSettingsResult>(new GetSiteSettingsQuery()));
        }
    }
}
