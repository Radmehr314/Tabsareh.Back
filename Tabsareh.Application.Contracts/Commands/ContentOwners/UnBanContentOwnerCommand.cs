using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.ContentOwners
{
    public class UnBanContentOwnerCommand : ICommand
    {
        public string Username { get; set; }
    }
}
