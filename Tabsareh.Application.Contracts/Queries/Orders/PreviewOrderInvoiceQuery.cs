using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Orders
{
    public class PreviewOrderInvoiceQuery : IQuery
    {
        public List<string> CourseIds { get; set; } = new();
        public string? DiscountCode { get; set; }
    }
}
