using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Users
{
    public class UpdateUserCommand : ICommand
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
    }
}
