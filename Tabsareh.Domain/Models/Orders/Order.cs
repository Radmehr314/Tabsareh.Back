namespace Tabsareh.Domain.Models.Orders
{
    public class Order : BaseEntity<string>
    {
        private readonly List<OrderItem> _items = new();

        private Order() { }

        public Order(
            string userId,
            string paymentMethod,
            decimal subtotalAmount,
            decimal courseDiscountAmount,
            decimal discountCodeAmount,
            decimal payableAmount,
            string? discountCode,
            decimal discountCodePercentSnapshot,
            string? cardToCardTrackingNumber,
            string? cardToCardDescription)
        {
            UserId = userId;
            PaymentMethod = paymentMethod;
            SubtotalAmount = subtotalAmount;
            CourseDiscountAmount = courseDiscountAmount;
            DiscountCodeAmount = discountCodeAmount;
            PayableAmount = payableAmount;
            DiscountCode = discountCode;
            DiscountCodePercentSnapshot = discountCodePercentSnapshot;
            CardToCardTrackingNumber = cardToCardTrackingNumber;
            CardToCardDescription = cardToCardDescription;
            Status = paymentMethod == OrderPaymentMethods.CardToCard
                ? OrderStatuses.PendingApproval
                : OrderStatuses.PendingPayment;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id)) throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void AddItem(OrderItem item) => _items.Add(item);

        public void SetGatewayToken(string token)
        {
            GatewayToken = token;
            UpdatedAt = DateTime.Now;
        }

        public void CompleteGatewayPayment(string refNum, string? rrn, string? traceNo, string? securePan)
        {
            GatewayRefNum = refNum;
            GatewayRRN = rrn;
            GatewayTraceNo = traceNo;
            GatewaySecurePan = securePan;
            Status = OrderStatuses.Success;
            PaidAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void Approve()
        {
            Status = OrderStatuses.Success;
            PaidAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void Reject(string? reason)
        {
            Status = OrderStatuses.Canceled;
            RejectionReason = reason;
            UpdatedAt = DateTime.Now;
        }

        public string UserId { get; private set; }
        public string? CourseId { get; private set; }
        public string PaymentMethod { get; private set; }
        public string Status { get; private set; }
        public decimal CoursePrice { get; private set; }
        public decimal DiscountPercentSnapshot { get; private set; }
        public decimal Amount { get; private set; }
        public decimal SettlementBasePriceSnapshot { get; private set; }
        public decimal ContentOwnerSharePercentSnapshot { get; private set; }
        public decimal SubtotalAmount { get; private set; }
        public decimal CourseDiscountAmount { get; private set; }
        public decimal DiscountCodeAmount { get; private set; }
        public decimal PayableAmount { get; private set; }
        public string? DiscountCode { get; private set; }
        public decimal DiscountCodePercentSnapshot { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public string? CardToCardTrackingNumber { get; private set; }
        public string? CardToCardDescription { get; private set; }
        public string? RejectionReason { get; private set; }
        public string? LicenseCode { get; private set; }
        public string? GatewayToken { get; private set; }
        public string? GatewayRefNum { get; private set; }
        public string? GatewayRRN { get; private set; }
        public string? GatewayTraceNo { get; private set; }
        public string? GatewaySecurePan { get; private set; }
        public IReadOnlyCollection<OrderItem> Items => _items;
    }
}
