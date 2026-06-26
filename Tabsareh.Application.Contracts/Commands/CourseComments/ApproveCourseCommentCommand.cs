using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.CourseComments
{
    public class ApproveCourseCommentCommand : ICommand
    {
        public string Id { get; set; } = string.Empty;
    }
}
