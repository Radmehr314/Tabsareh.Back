namespace Tabsareh.Application.Contracts.QueryResult.Orders
{
    public class OrderInvoiceResult
    {
        public List<OrderInvoiceItemResult> Items { get; set; } = new();
        public decimal SubtotalAmount { get; set; }
        public decimal CourseDiscountAmount { get; set; }
        public string? DiscountCode { get; set; }
        public decimal DiscountCodePercent { get; set; }
        public decimal DiscountCodeAmount { get; set; }
        public decimal LicensePrice { get; set; }
        public decimal PayableAmount { get; set; }
    }
}
