using MongoDB.Bson;
using Tabsareh.Domain.Models.Teachers;

namespace Tabsareh.Infrastructure.Persistance.MongoDocuments.TeacherMongo
{
    public static class TeacherMapper
    {
        public static Teacher ToDomain(this TeacherDocument doc)
        {
            var teacher = new Teacher(
                firstName: doc.FirstName,
                lastName: doc.LastName,
                profileImage: doc.ProfileImage,
                description: doc.Description
            );
            teacher.SetId(doc.Id.ToString());
            teacher.IsDeleted = doc.IsDeleted;
            teacher.CreatedAt = doc.CreatedAt;
            teacher.UpdatedAt = doc.UpdatedAt;
            return teacher;
        }

        public static List<Teacher> ToDomainList(this IEnumerable<TeacherDocument> documents)
        {
            if (documents == null)
                return new List<Teacher>();
            return documents.Select(doc => doc.ToDomain()).ToList();
        }

        public static TeacherDocument ToDocument(this Teacher domain)
        {
            if (domain == null)
                return null;

            var objectId = string.IsNullOrWhiteSpace(domain.Id)
                ? ObjectId.GenerateNewId()
                : ObjectId.Parse(domain.Id);

            return new TeacherDocument
            {
                Id = objectId,
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                ProfileImage = domain.ProfileImage,
                Description = domain.Description,
                IsDeleted = domain.IsDeleted,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt
            };
        }
    }
}
