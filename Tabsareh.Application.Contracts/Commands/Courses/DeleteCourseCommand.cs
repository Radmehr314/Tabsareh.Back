using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Courses
{
    public class DeleteCourseCommand : ICommand
    {
        public string Id { get; set; }
    }
}
