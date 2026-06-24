namespace Tabsareh.Application.Contracts.QueryResult.Category
{
    public class GetCategoriesPagedQueryResult
    {
        public List<CategoryItemResult> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
