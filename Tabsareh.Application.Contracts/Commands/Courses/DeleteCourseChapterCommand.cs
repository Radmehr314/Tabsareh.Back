using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Courses
{
    public class DeleteCourseChapterCommand : ICommand
    {
        public string Id { get; set; }
    }
}
