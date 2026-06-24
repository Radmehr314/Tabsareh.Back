namespace Tabsareh.Application.Contracts.QueryResult.Blog
{
    public class GetBlogsPagedQueryResult
    {
        public List<BlogItemResult> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
