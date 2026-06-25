namespace Tabsareh.Domain.Models.Orders
{
    public class OrderItem : BaseEntity<string>
    {
        private OrderItem() { }

        public OrderItem(
            string orderId,
            string courseId,
            string courseTitleSnapshot,
            decimal coursePriceSnapshot,
            decimal courseDiscountPercentSnapshot,
            decimal courseDiscountAmountSnapshot,
            decimal discountCodePercentSnapshot,
            decimal discountCodeAmountSnapshot,
            decimal finalAmount,
            decimal settlementBasePriceSnapshot,
            decimal contentOwnerSharePercentSnapshot,
            string contentOwnerIdSnapshot,
            string contentOwnerNameSnapshot)
        {
            OrderId = orderId;
            CourseId = courseId;
            CourseTitleSnapshot = courseTitleSnapshot;
            CoursePriceSnapshot = coursePriceSnapshot;
            CourseDiscountPercentSnapshot = courseDiscountPercentSnapshot;
            CourseDiscountAmountSnapshot = courseDiscountAmountSnapshot;
            DiscountCodePercentSnapshot = discountCodePercentSnapshot;
            DiscountCodeAmountSnapshot = discountCodeAmountSnapshot;
            FinalAmount = finalAmount;
            SettlementBasePriceSnapshot = settlementBasePriceSnapshot;
            ContentOwnerSharePercentSnapshot = contentOwnerSharePercentSnapshot;
            ContentOwnerIdSnapshot = contentOwnerIdSnapshot;
            ContentOwnerNameSnapshot = contentOwnerNameSnapshot;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id)) throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void SetLicense(string licenseCode)
        {
            LicenseCode = licenseCode;
            UpdatedAt = DateTime.Now;
        }

        public string OrderId { get; private set; }
        public string CourseId { get; private set; }
        public string CourseTitleSnapshot { get; private set; }
        public decimal CoursePriceSnapshot { get; private set; }
        public decimal CourseDiscountPercentSnapshot { get; private set; }
        public decimal CourseDiscountAmountSnapshot { get; private set; }
        public decimal DiscountCodePercentSnapshot { get; private set; }
        public decimal DiscountCodeAmountSnapshot { get; private set; }
        public decimal FinalAmount { get; private set; }
        public decimal SettlementBasePriceSnapshot { get; private set; }
        public decimal ContentOwnerSharePercentSnapshot { get; private set; }
        public string ContentOwnerIdSnapshot { get; private set; }
        public string ContentOwnerNameSnapshot { get; private set; }
        public string? LicenseCode { get; private set; }
    }
}
