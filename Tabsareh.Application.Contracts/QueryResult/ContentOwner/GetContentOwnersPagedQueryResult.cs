namespace Tabsareh.Application.Contracts.QueryResult.ContentOwner
{
    public class GetContentOwnersPagedQueryResult
    {
        public List<ContentOwnerItemResult> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
