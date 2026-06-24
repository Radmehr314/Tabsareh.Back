namespace Tabsareh.Application.Contracts.QueryResult.Role
{
    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Permissions { get; set; } = new();
        public string CreatedAt { get; set; }
    }
}
