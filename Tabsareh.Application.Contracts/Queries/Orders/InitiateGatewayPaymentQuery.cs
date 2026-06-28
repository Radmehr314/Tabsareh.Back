using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Orders
{
    public class InitiateGatewayPaymentQuery : IQuery
    {
        public string? DiscountCode { get; set; }
        public string? CellNumber { get; set; }
    }
}
