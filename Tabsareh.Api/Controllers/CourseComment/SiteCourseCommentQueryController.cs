using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabsareh.Application.Contracts.Queries.CourseComments;
using Tabsareh.Application.Contracts.QueryResult.CourseComments;
using Tabsareh.Framework.Api;
using Tabsareh.Framework.Application;

namespace Tabsareh.Api.Controllers.CourseComment
{
    [AllowAnonymous]
    public class SiteCourseCommentQueryController : BaseQueryController
    {
        public SiteCourseCommentQueryController(IQueryBus bus) : base(bus) { }

        /// <summary>
        /// دریافت لیست کامنت‌های تأیید شده یک دوره به همراه میانگین امتیاز.
        /// </summary>
        [HttpGet("PublicCourseComments")]
        public async Task<ActionResult<GetPublicCourseCommentsQueryResult>> GetPublicComments([FromQuery] GetPublicCourseCommentsQuery query)
            => Ok(await Bus.Dispatch<GetPublicCourseCommentsQuery, GetPublicCourseCommentsQueryResult>(query));
    }
}
