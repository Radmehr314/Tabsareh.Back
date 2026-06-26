namespace Tabsareh.Application.Contracts.QueryResult.ContentOwner
{
    public class ContentOwnerPaymentItemResult
    {
        public string Id { get; set; }
        public string ContentOwnerId { get; set; }
        public string ContentOwnerName { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentDatePersian { get; set; }
        public string ReceiptImage { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Description { get; set; }
        public string CreatedAt { get; set; }
    }
}
