using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.Auth;
using Tabsareh.Application.Contracts.QueryResult.Auth;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Auth
{
    public class AuthQueryController : BaseQueryController
    {
        public AuthQueryController(IQueryBus bus) : base(bus) { }

        [HttpPost("LoginAdmin")]
        public async Task<ActionResult<LoginDto>> LoginAdmin([FromBody] LoginAdminQuery query)
            => Ok(await Bus.Dispatch<LoginAdminQuery, LoginDto>(query));

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResultDto>> Login([FromBody] LoginQuery query)
            => Ok(await Bus.Dispatch<LoginQuery, LoginResultDto>(query));

        [HttpPost("RequestUserOtp")]
        public async Task<ActionResult<RequestUserOtpResultDto>> RequestUserOtp([FromBody] RequestUserOtpQuery query)
            => Ok(await Bus.Dispatch<RequestUserOtpQuery, RequestUserOtpResultDto>(query));

        [HttpPost("VerifyUserOtp")]
        public async Task<ActionResult<LoginResultDto>> VerifyUserOtp([FromBody] VerifyUserOtpQuery query)
            => Ok(await Bus.Dispatch<VerifyUserOtpQuery, LoginResultDto>(query));

        [HttpGet("Me")]
        [Authorize]
        public async Task<ActionResult<CurrentUserDto>> Me()
            => Ok(await Bus.Dispatch<GetCurrentUserQuery, CurrentUserDto>(new GetCurrentUserQuery()));
    }
}
