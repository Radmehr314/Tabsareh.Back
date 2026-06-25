namespace Tabsareh.Application.Contracts.QueryResult.Course
{
    public class CourseChapterVideoResult
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string? ExternalVideoId { get; set; }
        public string? VideoUrl { get; set; }
        public string UploadStatus { get; set; }
    }
}
