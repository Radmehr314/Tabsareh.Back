using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Commands.SiteSettings;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.SiteSettings
{
    public class SiteSettingCommandController : BaseCommandController
    {
        public SiteSettingCommandController(ICommandBus bus) : base(bus) { }

        [HttpPut("UpdateLicensePrice")]
        [Authorize(Policy = "manage_courses")]
        public async Task<IActionResult> UpdateLicensePrice([FromBody] UpdateLicensePriceCommand command)
        {
            return Ok(await Bus.Dispatch(command));
        }
    }
}
