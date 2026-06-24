using MongoDB.Bson.Serialization.Attributes;

namespace Tabsareh.Infrastructure.Persistance.MongoDocuments.ContentOwnerMongo
{
    [BsonIgnoreExtraElements]
    public class ContentOwnerDocument : BaseDocument
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsBan { get; set; }
        public bool IsDeleted { get; set; }
        public List<string> TeacherIds { get; set; } = new();
    }
}
