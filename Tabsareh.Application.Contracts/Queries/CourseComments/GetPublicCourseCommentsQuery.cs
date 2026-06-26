using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.CourseComments
{
    public class GetPublicCourseCommentsQuery : IQuery
    {
        public string CourseId { get; set; } = string.Empty;
    }
}
