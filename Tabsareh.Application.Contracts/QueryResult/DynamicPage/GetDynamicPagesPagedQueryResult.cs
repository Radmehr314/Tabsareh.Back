namespace Tabsareh.Application.Contracts.QueryResult.DynamicPage
{
    public class GetDynamicPagesPagedQueryResult
    {
        public List<DynamicPageItemResult> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
