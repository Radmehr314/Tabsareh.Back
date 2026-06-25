namespace Tabsareh.Domain.Models.Discounts
{
    public class DiscountCode : BaseEntity<string>
    {
        private DiscountCode() { }

        public DiscountCode(string title, string code, int usageLimit, decimal discountPercent, DateTime startDate, DateTime endDate)
        {
            Title = title;
            Code = code.Trim().ToUpperInvariant();
            UsageLimit = usageLimit;
            DiscountPercent = discountPercent;
            StartDate = startDate.Date;
            EndDate = endDate.Date;
        }

        public void SetId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id is required.", nameof(id));
            if (!string.IsNullOrWhiteSpace(Id)) throw new InvalidOperationException("Id is already set.");
            Id = id;
        }

        public void Update(string title, string code, int usageLimit, decimal discountPercent, DateTime startDate, DateTime endDate)
        {
            Title = title;
            Code = code.Trim().ToUpperInvariant();
            UsageLimit = usageLimit;
            DiscountPercent = discountPercent;
            StartDate = startDate.Date;
            EndDate = endDate.Date;
            UpdatedAt = DateTime.Now;
        }

        public void Delete() => IsDeleted = true;

        public string Title { get; private set; }
        public string Code { get; private set; }
        public int UsageLimit { get; private set; }
        public int UsedCount { get; private set; }
        public decimal DiscountPercent { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsDeleted { get; private set; }
    }
}
