using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;
using Tabsareh.Application.Contracts.Queries.Teacher;
using Tabsareh.Application.Contracts.QueryResult.Teacher;

namespace Tabsareh.Api.Controllers.Teacher
{
    [Authorize(Policy = "manage_teachers")]
    public class TeacherQueryController : BaseQueryController
    {
        public TeacherQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("Teachers")]
        public async Task<ActionResult<GetTeachersPagedQueryResult>> GetTeachers([FromQuery] GetTeachersPagedQuery query)
        {
            return Ok(await Bus.Dispatch<GetTeachersPagedQuery, GetTeachersPagedQueryResult>(query));
        }

        [HttpGet("AllTeachersWithoutPagination")]
        public async Task<ActionResult<List<TeacherItemResult>>> GetAllTeachers([FromQuery] GetAllTeachersWithoutPaginationQuery query)
        {
            return Ok(await Bus.Dispatch<GetAllTeachersWithoutPaginationQuery, List<TeacherItemResult>>(query));
        }

        [HttpGet("GetTeacher")]
        public async Task<ActionResult<TeacherItemResult>> GetTeacher([FromQuery] GetTeacherByIdQuery query)
        {
            return Ok(await Bus.Dispatch<GetTeacherByIdQuery, TeacherItemResult>(query));
        }
    }
}
