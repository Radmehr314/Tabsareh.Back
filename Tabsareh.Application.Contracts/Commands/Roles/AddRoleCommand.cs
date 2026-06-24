using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Roles
{
    public class AddRoleCommand : ICommand
    {
        public string Name { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
