namespace Tabsareh.Domain.Models.Orders
{
    public class OrderFilter
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ContentOwnerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
