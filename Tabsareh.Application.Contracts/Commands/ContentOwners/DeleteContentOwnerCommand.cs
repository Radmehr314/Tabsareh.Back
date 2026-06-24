using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.ContentOwners
{
    public class DeleteContentOwnerCommand : ICommand
    {
        public string Id { get; set; }
    }
}
