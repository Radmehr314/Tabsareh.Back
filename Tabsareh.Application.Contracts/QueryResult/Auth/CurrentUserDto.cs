namespace Tabsareh.Application.Contracts.QueryResult.Auth
{
    public class CurrentUserDto
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }

        public string? Phone { get; set; }
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
