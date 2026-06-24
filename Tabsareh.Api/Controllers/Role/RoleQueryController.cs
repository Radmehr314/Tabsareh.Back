using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;
using Tabsareh.Application.Contracts.Queries.Role;
using Tabsareh.Application.Contracts.QueryResult.Role;

namespace Tabsareh.Api.Controllers.Role
{
    [Authorize(Policy = "manage_roles")]
    public class RoleQueryController : BaseQueryController
    {
        public RoleQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("AllRoles")]
        public async Task<ActionResult<List<RoleDto>>> GetAllRoles()
        {
            return Ok(await Bus.Dispatch<GetAllRolesQuery, List<RoleDto>>(new GetAllRolesQuery()));
        }

        [HttpGet("Roles")]
        public async Task<ActionResult<GetRolesPagedQueryResult>> GetRolesPaged([FromQuery] GetRolesPagedQuery query)
        {
            return Ok(await Bus.Dispatch<GetRolesPagedQuery, GetRolesPagedQueryResult>(query));
        }

        [HttpGet("GetRole")]
        public async Task<ActionResult<RoleDto>> GetRole([FromQuery] GetRoleByIdQuery query)
        {
            return Ok(await Bus.Dispatch<GetRoleByIdQuery, RoleDto>(query));
        }

        [HttpGet("AllPermissions")]
        public async Task<ActionResult<List<string>>> GetAllPermissions()
        {
            return Ok(await Bus.Dispatch<GetAllPermissionsQuery, List<string>>(new GetAllPermissionsQuery()));
        }
    }
}
