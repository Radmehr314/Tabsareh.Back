namespace Tabsareh.Application.Contracts.QueryResult.Admin;

public class GetAdminInfoByTokenQueryResult
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Phone { get; set; }
    public string? RoleId { get; set; }
    public string? RoleName { get; set; }
    public List<string> Permissions { get; set; } = new();
}
