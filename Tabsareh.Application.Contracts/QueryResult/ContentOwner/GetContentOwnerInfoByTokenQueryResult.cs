namespace Tabsareh.Application.Contracts.QueryResult.ContentOwner
{
    public class GetContentOwnerInfoByTokenQueryResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public List<string> TeacherIds { get; set; } = new();
        public List<TeacherBriefDto> Teachers { get; set; } = new();
    }
}
