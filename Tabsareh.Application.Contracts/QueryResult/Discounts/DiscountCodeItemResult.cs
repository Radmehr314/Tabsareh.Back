namespace Tabsareh.Application.Contracts.QueryResult.Discounts
{
    public class DiscountCodeItemResult
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartDatePersian { get; set; }
        public string EndDatePersian { get; set; }
        public string CreatedAt { get; set; }
    }
}
