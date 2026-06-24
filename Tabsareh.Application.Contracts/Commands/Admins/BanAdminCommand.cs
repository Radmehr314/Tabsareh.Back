using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Admins;

public class BanAdminCommand : ICommand
{
    public string Username { get; set; }
}
