namespace Tabsareh.Application.Contracts.Commands.Courses
{
    public class CourseChapterVideoCommandItem
    {
        public string Title { get; set; }
        public string Duration { get; set; }
        public string? ExternalVideoId { get; set; }
        public string? VideoUrl { get; set; }
        public string UploadStatus { get; set; } = "PendingExternalUpload";
    }
}
