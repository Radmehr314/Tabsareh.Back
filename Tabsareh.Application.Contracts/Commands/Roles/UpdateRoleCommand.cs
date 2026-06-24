using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Roles
{
    public class UpdateRoleCommand : ICommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
