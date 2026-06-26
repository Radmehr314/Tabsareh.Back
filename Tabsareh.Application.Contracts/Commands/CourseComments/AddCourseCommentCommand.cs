using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.CourseComments
{
    public class AddCourseCommentCommand : ICommand
    {
        public string CourseId { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorPhone { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
