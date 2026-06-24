using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Users
{
    public class DeleteUserCommand : ICommand
    {
        public string Id { get; set; }
    }
}
