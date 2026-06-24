using MongoDB.Bson.Serialization.Attributes;

namespace Tabsareh.Infrastructure.Persistance.MongoDocuments.TeacherMongo
{
    [BsonIgnoreExtraElements]
    public class TeacherDocument : BaseDocument
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImage { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
