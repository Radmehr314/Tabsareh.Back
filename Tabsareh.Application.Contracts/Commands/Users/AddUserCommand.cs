using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Users
{
    public class AddUserCommand : ICommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
    }
}
