namespace Tabsareh.Application.Contracts.QueryResult.Discounts
{
    public class GetDiscountCodesPagedQueryResult
    {
        public List<DiscountCodeItemResult> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
