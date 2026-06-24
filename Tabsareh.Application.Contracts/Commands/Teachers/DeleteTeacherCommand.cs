using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Teachers
{
    public class DeleteTeacherCommand : ICommand
    {
        public string Id { get; set; }
    }
}
