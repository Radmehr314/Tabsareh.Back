namespace Tabsareh.Application.Contracts.QueryResult.Teacher
{
    public class GetTeachersPagedQueryResult
    {
        public List<TeacherItemResult> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
