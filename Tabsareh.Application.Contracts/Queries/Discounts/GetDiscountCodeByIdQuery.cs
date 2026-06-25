using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Discounts
{
    public class GetDiscountCodeByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
