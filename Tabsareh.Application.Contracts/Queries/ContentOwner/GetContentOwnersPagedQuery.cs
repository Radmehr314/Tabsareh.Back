using Tabsareh.Framework.Application;
using Tabsareh.Domain.Common;

namespace Tabsareh.Application.Contracts.Queries.ContentOwner
{
    public class GetContentOwnersPagedQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
    }
}
