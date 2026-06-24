using Tabsareh.Framework.Application;
using Tabsareh.Domain.Common;

namespace Tabsareh.Application.Contracts.Queries.Role
{
    public class GetRolesPagedQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
    }
}
