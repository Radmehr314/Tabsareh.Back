using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.CourseComments;
using Tabsareh.Application.Contracts.QueryResult.CourseComments;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.CourseComment
{
    [Authorize(Policy = "manage_comments")]
    public class CourseCommentQueryController : BaseQueryController
    {
        public CourseCommentQueryController(IQueryBus bus) : base(bus) { }

        [HttpGet("CourseComments")]
        public async Task<ActionResult<GetCourseCommentsPagedQueryResult>> GetCourseComments([FromQuery] GetCourseCommentsPagedQuery query)
            => Ok(await Bus.Dispatch<GetCourseCommentsPagedQuery, GetCourseCommentsPagedQueryResult>(query));
    }
}
