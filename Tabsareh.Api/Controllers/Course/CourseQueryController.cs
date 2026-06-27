using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.Course;
using Tabsareh.Application.Contracts.QueryResult.Course;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Course
{
    [Authorize(Policy = "manage_courses")]
    public class CourseQueryController : BaseQueryController
    {
        public CourseQueryController(IQueryBus bus) : base(bus) { }

        [AllowAnonymous]
        [HttpGet("PublicCourses")]
        public async Task<ActionResult<PublicCoursesResult>> GetPublicCourses()
            => Ok(await Bus.Dispatch<GetPublicCoursesQuery, PublicCoursesResult>(new GetPublicCoursesQuery()));

        [HttpGet("Courses")]
        public async Task<ActionResult<GetCoursesPagedQueryResult>> GetCoursesPaged([FromQuery] GetCoursesPagedQuery query)
        {
            return Ok(await Bus.Dispatch<GetCoursesPagedQuery, GetCoursesPagedQueryResult>(query));
        }

        [HttpGet("AllCourses")]
        public async Task<ActionResult<List<CourseItemResult>>> GetAllCourses()
        {
            return Ok(await Bus.Dispatch<GetAllCoursesQuery, List<CourseItemResult>>(new GetAllCoursesQuery()));
        }

        [HttpGet("GetCourse")]
        public async Task<ActionResult<CourseItemResult>> GetCourse([FromQuery] GetCourseByIdQuery query)
        {
            return Ok(await Bus.Dispatch<GetCourseByIdQuery, CourseItemResult>(query));
        }
    }
}
