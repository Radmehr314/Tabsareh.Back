using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.Course;
using Tabsareh.Application.Contracts.QueryResult.Course;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.Course
{
    [Authorize(Policy = "manage_course_chapters")]
    public class CourseChapterQueryController : BaseQueryController
    {
        public CourseChapterQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("CourseChapters")]
        public async Task<ActionResult<List<CourseChapterItemResult>>> GetCourseChapters([FromQuery] GetCourseChaptersByCourseIdQuery query)
        {
            return Ok(await Bus.Dispatch<GetCourseChaptersByCourseIdQuery, List<CourseChapterItemResult>>(query));
        }
    }
}
