using Tabsareh.Domain.Common;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Discounts
{
    public class GetDiscountCodesPagedQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
    }
}
