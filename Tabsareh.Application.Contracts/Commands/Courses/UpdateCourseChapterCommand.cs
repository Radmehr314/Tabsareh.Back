using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Courses
{
    public class UpdateCourseChapterCommand : ICommand
    {
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public int DisplayOrder { get; set; }
        public List<CourseChapterVideoCommandItem> Videos { get; set; } = new();
    }
}
