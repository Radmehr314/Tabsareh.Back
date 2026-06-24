using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Admins;

public class UpdateAdminCommand : ICommand
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? RoleId { get; set; }
}
