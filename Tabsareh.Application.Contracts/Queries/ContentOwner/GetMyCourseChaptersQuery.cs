using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.ContentOwner
{
    public class GetMyCourseChaptersQuery : IQuery
    {
        public string CourseId { get; set; } = string.Empty;
    }
}
