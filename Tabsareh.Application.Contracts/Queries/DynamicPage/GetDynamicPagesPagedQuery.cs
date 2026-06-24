using Tabsareh.Domain.Common;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.DynamicPage
{
    public class GetDynamicPagesPagedQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
    }
}
