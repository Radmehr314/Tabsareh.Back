namespace Tabsareh.Application.Contracts.QueryResult.ContentOwner
{
    public class ContentOwnerItemResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public bool IsBan { get; set; }
        public List<string> TeacherIds { get; set; } = new();
        public List<TeacherBriefDto> Teachers { get; set; } = new();
        public string CreatedAt { get; set; }
    }
}
