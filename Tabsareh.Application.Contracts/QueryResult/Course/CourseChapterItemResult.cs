namespace Tabsareh.Application.Contracts.QueryResult.Course
{
    public class CourseChapterItemResult
    {
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string? CourseTitle { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public int DisplayOrder { get; set; }
        public string CreatedAt { get; set; }
        public List<CourseChapterVideoResult> Videos { get; set; } = new();
    }
}
