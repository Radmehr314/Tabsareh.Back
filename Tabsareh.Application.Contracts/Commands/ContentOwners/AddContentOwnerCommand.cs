using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.ContentOwners
{
    public class AddContentOwnerCommand : ICommand
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsBan { get; set; } = false;
    }
}
