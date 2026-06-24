using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;
using Tabsareh.Application.Contracts.Queries.Admin;
using Tabsareh.Application.Contracts.QueryResult.Admin;

namespace Tabsareh.Api.Controllers.Admin
{
    public class AdminQueryController : BaseQueryController
    {
        public AdminQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("AllAdmins")]
        [Authorize(Policy = "manage_admins")]
        public async Task<ActionResult<GetAllAdminQueryResult>> GetAllAdmins([FromQuery] GetAllAdminQuery query)
        {
            return Ok(await Bus.Dispatch<GetAllAdminQuery, GetAllAdminQueryResult>(query));
        }

        [HttpGet("AllAdminsWithoutPagination")]
        [Authorize(Policy = "manage_admins")]
        public async Task<ActionResult<List<GetAllAdminItemResult>>> GetAllAdminsWithoutPagination([FromQuery] GetAllAdminsWithoutPaginationQuery query)
        {
            return Ok(await Bus.Dispatch<GetAllAdminsWithoutPaginationQuery, List<GetAllAdminItemResult>>(query));
        }

        [HttpGet("GetAdmin")]
        [Authorize(Policy = "manage_admins")]
        public async Task<ActionResult<GetAllAdminItemResult>> GetAdmin([FromQuery] GetAdminByIdQuery query)
        {
            return Ok(await Bus.Dispatch<GetAdminByIdQuery, GetAllAdminItemResult>(query));
        }

        [HttpGet("GetLoginAdminInfo")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<GetAdminInfoByTokenQueryResult>> GetLoginAdminInfo([FromQuery] GetAdminInfoByTokenQuery query)
        {
            return Ok(await Bus.Dispatch<GetAdminInfoByTokenQuery, GetAdminInfoByTokenQueryResult>(query));
        }
    }
}
