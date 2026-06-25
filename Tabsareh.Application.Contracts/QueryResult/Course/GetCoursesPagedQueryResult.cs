namespace Tabsareh.Application.Contracts.QueryResult.Course
{
    public class GetCoursesPagedQueryResult
    {
        public List<CourseItemResult> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
