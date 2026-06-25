using Tabsareh.Domain.Common;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Course
{
    public class GetCoursesPagedQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
    }
}
