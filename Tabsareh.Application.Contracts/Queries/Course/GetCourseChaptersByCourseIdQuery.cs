using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Course
{
    public class GetCourseChaptersByCourseIdQuery : IQuery
    {
        public string CourseId { get; set; }
    }
}
