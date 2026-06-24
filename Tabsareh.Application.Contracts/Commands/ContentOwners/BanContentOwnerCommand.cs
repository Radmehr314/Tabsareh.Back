using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.ContentOwners
{
    public class BanContentOwnerCommand : ICommand
    {
        public string Username { get; set; }
    }
}
