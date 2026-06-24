using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;
using Tabsareh.Application.Contracts.Queries.Auth;
using Tabsareh.Application.Contracts.QueryResult.Auth;

namespace Tabsareh.Api.Controllers.Auth
{
    public class AuthQueryController : BaseQueryController
    {
        public AuthQueryController(IQueryBus bus) : base(bus) { }

        [HttpPost("LoginAdmin")]
        public async Task<ActionResult<LoginDto>> LoginAdmin([FromBody] LoginAdminQuery query)
        {
            return Ok(await Bus.Dispatch<LoginAdminQuery, LoginDto>(query));
        }

        /// <summary>ورود یکپارچه برای ادمین‌ها و صاحبان اثر.</summary>
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResultDto>> Login([FromBody] LoginQuery query)
        {
            return Ok(await Bus.Dispatch<LoginQuery, LoginResultDto>(query));
        }

        /// <summary>اطلاعات کاربر لاگین‌شده (ادمین یا صاحب اثر) برای نمایش در پنل.</summary>
        [HttpGet("Me")]
        [Authorize]
        public async Task<ActionResult<CurrentUserDto>> Me()
        {
            return Ok(await Bus.Dispatch<GetCurrentUserQuery, CurrentUserDto>(new GetCurrentUserQuery()));
        }
    }
}
