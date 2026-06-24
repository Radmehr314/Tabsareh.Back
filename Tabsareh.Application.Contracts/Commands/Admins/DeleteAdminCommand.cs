using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Admins;

public class DeleteAdminCommand : ICommand
{
    public string Id { get; set; }
}
