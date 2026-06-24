namespace Tabsareh.Application.Contracts.QueryResult.Teacher
{
    public class TeacherItemResult
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImage { get; set; }
        public string? Description { get; set; }
        public string CreatedAt { get; set; }
    }
}
