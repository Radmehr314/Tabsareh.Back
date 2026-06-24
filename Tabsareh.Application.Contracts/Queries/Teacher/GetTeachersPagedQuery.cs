using Tabsareh.Framework.Application;
using Tabsareh.Domain.Common;

namespace Tabsareh.Application.Contracts.Queries.Teacher
{
    public class GetTeachersPagedQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
    }
}
