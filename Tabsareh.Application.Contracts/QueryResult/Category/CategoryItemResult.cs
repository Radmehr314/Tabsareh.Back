namespace Tabsareh.Application.Contracts.QueryResult.Category
{
    public class CategoryItemResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ParentId { get; set; }
        public string? ParentName { get; set; }
        public int Level { get; set; }
        public string CreatedAt { get; set; }
    }
}
