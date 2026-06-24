namespace Tabsareh.Application.Contracts.QueryResult.Admin;

public class GetAllAdminQueryResult
{
    public List<GetAllAdminItemResult> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Skip { get; set; }
    public int Limit { get; set; }
}
