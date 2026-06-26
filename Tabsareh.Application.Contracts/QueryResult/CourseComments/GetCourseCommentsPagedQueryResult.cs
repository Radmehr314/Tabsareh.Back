namespace Tabsareh.Application.Contracts.QueryResult.CourseComments
{
    public class GetCourseCommentsPagedQueryResult
    {
        public List<CourseCommentResult> Items { get; set; } = new();
        public long TotalCount { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
