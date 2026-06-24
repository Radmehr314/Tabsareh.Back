using MongoDB.Bson;
using Tabsareh.Domain.Models.Admins;

namespace Tabsareh.Infrastructure.Persistance.MongoDocuments.AdminMongo
{
    public static class AdminMapper
    {
        public static Admin ToDomain(this AdminDocument doc)
        {
            var user = new Admin(
                userName: doc.UserName,
                firstName: doc.FirstName,
                lastName: doc.LastName,
                phone: doc.Phone,
                password: doc.Password,
                salt: doc.Salt,
                isBan: doc.IsBan,
                roleId: doc.RoleId
            );
            user.SetId(doc.Id.ToString());
            return user;
        }

        public static List<Admin> ToDomainList(this IEnumerable<AdminDocument> documents)
        {
            if (documents == null)
                return new List<Admin>();
            return documents.Select(doc => doc.ToDomain()).ToList();
        }

        public static AdminDocument ToDocument(this Admin domain)
        {
            if (domain == null)
                return null;

            var objectId = string.IsNullOrWhiteSpace(domain.Id)
                ? ObjectId.GenerateNewId()
                : ObjectId.Parse(domain.Id);

            return new AdminDocument
            {
                Id = objectId,
                UserName = domain.UserName,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                Phone = domain.Phone,
                Password = domain.Password,
                Salt = domain.Salt,
                IsBan = domain.IsBan,
                RoleId = domain.RoleId,
                IsDeleted = domain.IsDeleted,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt
            };
        }
    }
}
