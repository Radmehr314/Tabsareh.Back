using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Admins
{
    public class AddAdminCommand : ICommand
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsBan { get; set; } = false;
        public string? RoleId { get; set; }
    }
}
