using MongoDB.Bson.Serialization.Attributes;

namespace Tabsareh.Infrastructure.Persistance.MongoDocuments.AdminMongo
{
    [BsonIgnoreExtraElements]
    public class AdminDocument : BaseDocument
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsBan { get; set; }
        public string? RoleId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
