using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Roles
{
    public class DeleteRoleCommand : ICommand
    {
        public string Id { get; set; }
    }
}
