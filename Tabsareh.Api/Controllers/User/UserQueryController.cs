using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.Users;
using Tabsareh.Application.Contracts.QueryResult.Users;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.User
{
    [Authorize(Policy = "manage_users")]
    public class UserQueryController : BaseQueryController
    {
        public UserQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("Users")]
        public async Task<ActionResult<GetUsersPagedQueryResult>> GetUsersPaged([FromQuery] GetUsersPagedQuery query)
        {
            return Ok(await Bus.Dispatch<GetUsersPagedQuery, GetUsersPagedQueryResult>(query));
        }

        [HttpGet("AllUsers")]
        public async Task<ActionResult<List<UserItemResult>>> GetAllUsers()
        {
            return Ok(await Bus.Dispatch<GetAllUsersQuery, List<UserItemResult>>(new GetAllUsersQuery()));
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<UserItemResult>> GetUser([FromQuery] GetUserByIdQuery query)
        {
            return Ok(await Bus.Dispatch<GetUserByIdQuery, UserItemResult>(query));
        }
    }
}
