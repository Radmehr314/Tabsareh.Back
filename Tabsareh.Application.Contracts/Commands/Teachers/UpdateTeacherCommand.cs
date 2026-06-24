using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Teachers
{
    public class UpdateTeacherCommand : ICommand
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImage { get; set; }
        public string? Description { get; set; }
    }
}
