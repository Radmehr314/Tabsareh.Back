using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.ContentOwners
{
    public class UpdateContentOwnerCommand : ICommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public List<string> TeacherIds { get; set; } = new();
    }
}
