using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Course
{
    public class GetCourseByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
