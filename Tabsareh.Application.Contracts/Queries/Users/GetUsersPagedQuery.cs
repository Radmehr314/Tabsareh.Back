using Tabsareh.Domain.Common;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Users
{
    public class GetUsersPagedQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
    }
}
