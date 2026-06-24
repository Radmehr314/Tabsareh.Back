namespace Tabsareh.Application.Contracts.QueryResult.Admin;

public class GetAllAdminItemResult
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public bool IsBan { get; set; }
    public string? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string CreatedAt { get; set; }
}
