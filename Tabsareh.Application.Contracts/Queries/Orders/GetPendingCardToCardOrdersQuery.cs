using Tabsareh.Domain.Common;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Orders
{
    public class GetPendingCardToCardOrdersQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
        public string? Search { get; set; }
    }
}
