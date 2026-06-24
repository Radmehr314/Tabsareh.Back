namespace Tabsareh.Application.Contracts.QueryResult.Users
{
    public class GetUsersPagedQueryResult
    {
        public List<UserItemResult> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
