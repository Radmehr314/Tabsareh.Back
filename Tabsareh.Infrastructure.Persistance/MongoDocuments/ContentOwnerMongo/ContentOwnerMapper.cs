using MongoDB.Bson;
using Tabsareh.Domain.Models.ContentOwners;

namespace Tabsareh.Infrastructure.Persistance.MongoDocuments.ContentOwnerMongo
{
    public static class ContentOwnerMapper
    {
        public static ContentOwner ToDomain(this ContentOwnerDocument doc)
        {
            var owner = new ContentOwner(
                name: doc.Name,
                userName: doc.UserName,
                password: doc.Password,
                salt: doc.Salt,
                isBan: doc.IsBan,
                teacherIds: doc.TeacherIds
            );
            owner.SetId(doc.Id.ToString());
            owner.IsDeleted = doc.IsDeleted;
            owner.CreatedAt = doc.CreatedAt;
            owner.UpdatedAt = doc.UpdatedAt;
            return owner;
        }

        public static List<ContentOwner> ToDomainList(this IEnumerable<ContentOwnerDocument> documents)
        {
            if (documents == null)
                return new List<ContentOwner>();
            return documents.Select(doc => doc.ToDomain()).ToList();
        }

        public static ContentOwnerDocument ToDocument(this ContentOwner domain)
        {
            if (domain == null)
                return null;

            var objectId = string.IsNullOrWhiteSpace(domain.Id)
                ? ObjectId.GenerateNewId()
                : ObjectId.Parse(domain.Id);

            return new ContentOwnerDocument
            {
                Id = objectId,
                Name = domain.Name,
                UserName = domain.UserName,
                Password = domain.Password,
                Salt = domain.Salt,
                IsBan = domain.IsBan,
                IsDeleted = domain.IsDeleted,
                TeacherIds = domain.TeacherIds,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt
            };
        }
    }
}
