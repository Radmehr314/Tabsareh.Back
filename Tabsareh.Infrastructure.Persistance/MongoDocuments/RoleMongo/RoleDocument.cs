namespace Tabsareh.Infrastructure.Persistance.MongoDocuments.RoleMongo
{
    public class RoleDocument : BaseDocument
    {
        public string Name { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
