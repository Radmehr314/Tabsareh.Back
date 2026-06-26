namespace Tabsareh.Application.Contracts.QueryResult.CourseComments
{
    public class CourseCommentResult
    {
        public string Id { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorPhone { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public bool IsApproved { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }
}
