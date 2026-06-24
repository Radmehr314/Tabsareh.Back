using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Admins;

public class UnBanAdminCommand : ICommand
{
    public string Username { get; set; }
}
