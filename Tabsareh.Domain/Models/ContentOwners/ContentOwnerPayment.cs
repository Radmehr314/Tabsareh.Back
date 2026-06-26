namespace Tabsareh.Domain.Models.ContentOwners
{
    public class ContentOwnerPayment : BaseEntity<string>
    {
        private ContentOwnerPayment() { }

        public ContentOwnerPayment(
            string contentOwnerId,
            decimal amount,
            DateTime paymentDate,
            string receiptImage,
            string? trackingNumber,
            string? description)
        {
            ContentOwnerId = contentOwnerId;
            Amount = amount;
            PaymentDate = paymentDate;
            ReceiptImage = receiptImage;
            TrackingNumber = trackingNumber;
            Description = description;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id)) throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public string ContentOwnerId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public string ReceiptImage { get; private set; }
        public string? TrackingNumber { get; private set; }
        public string? Description { get; private set; }
    }
}
