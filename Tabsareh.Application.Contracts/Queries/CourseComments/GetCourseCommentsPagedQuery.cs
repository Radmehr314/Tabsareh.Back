using Tabsareh.Domain.Common;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.CourseComments
{
    public class GetCourseCommentsPagedQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
        public string? CourseId { get; set; }
        public bool? IsApproved { get; set; }
    }
}
