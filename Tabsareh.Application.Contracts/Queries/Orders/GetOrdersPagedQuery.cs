using Tabsareh.Domain.Common;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Orders
{
    public class GetOrdersPagedQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
        public string? Search { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ContentOwnerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
