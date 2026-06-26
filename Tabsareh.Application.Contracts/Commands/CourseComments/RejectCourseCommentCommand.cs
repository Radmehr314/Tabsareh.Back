using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.CourseComments
{
    public class RejectCourseCommentCommand : ICommand
    {
        public string Id { get; set; } = string.Empty;
    }
}
