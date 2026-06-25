namespace Tabsareh.Application.Contracts.QueryResult.Orders
{
    public class OrderDetailItemResult
    {
        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public decimal CoursePrice { get; set; }
        public decimal CourseDiscountPercent { get; set; }
        public decimal CourseDiscountAmount { get; set; }
        public decimal DiscountCodePercent { get; set; }
        public decimal DiscountCodeAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string ContentOwnerId { get; set; }
        public string ContentOwnerName { get; set; }
        public string? LicenseCode { get; set; }
    }
}
