namespace Tabsareh.Application.Contracts.QueryResult.CourseComments
{
    public class GetPublicCourseCommentsQueryResult
    {
        public List<PublicCourseCommentResult> Comments { get; set; } = new();
        public double? AverageRating { get; set; }
        public int CommentCount { get; set; }
    }

    public class PublicCourseCommentResult
    {
        public string Id { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }
}
