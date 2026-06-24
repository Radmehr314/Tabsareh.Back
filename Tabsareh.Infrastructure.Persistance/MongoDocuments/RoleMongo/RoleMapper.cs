using MongoDB.Bson;
using Tabsareh.Domain.Models.Roles;

namespace Tabsareh.Infrastructure.Persistance.MongoDocuments.RoleMongo
{
    public static class RoleMapper
    {
        public static Role ToDomain(this RoleDocument doc)
        {
            var role = new Role(doc.Name, doc.Permissions);
            role.SetId(doc.Id.ToString());
            return role;
        }

        public static RoleDocument ToDocument(this Role domain)
        {
            if (domain == null)
                return null;

            var objectId = string.IsNullOrWhiteSpace(domain.Id)
                ? ObjectId.GenerateNewId()
                : ObjectId.Parse(domain.Id);

            return new RoleDocument
            {
                Id = objectId,
                Name = domain.Name,
                Permissions = domain.Permissions,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
            };
        }
    }
}
