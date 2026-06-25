namespace Tabsareh.Application.Contracts.QueryResult.Orders
{
    public class OrderItemResult
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserPhone { get; set; }
        public string? UserFullName { get; set; }
        public string? CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string? ContentOwnerId { get; set; }
        public string ContentOwnerName { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodTitle { get; set; }
        public string Status { get; set; }
        public string StatusTitle { get; set; }
        public decimal CoursePrice { get; set; }
        public decimal DiscountPercentSnapshot { get; set; }
        public decimal Amount { get; set; }
        public decimal SettlementBasePriceSnapshot { get; set; }
        public decimal ContentOwnerSharePercentSnapshot { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal CourseDiscountAmount { get; set; }
        public string? DiscountCode { get; set; }
        public decimal DiscountCodePercentSnapshot { get; set; }
        public decimal DiscountCodeAmount { get; set; }
        public decimal PayableAmount { get; set; }
        public string CreatedAt { get; set; }
        public string? PaidAt { get; set; }
        public string? CardToCardTrackingNumber { get; set; }
        public string? CardToCardDescription { get; set; }
        public string? RejectionReason { get; set; }
        public string? LicenseCode { get; set; }
        public List<OrderDetailItemResult> Items { get; set; } = new();
    }
}
