using MongoDB.Bson;
using MongoDB.Driver;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Teachers;
using Tabsareh.Infrastructure.Persistance.Common;
using Tabsareh.Infrastructure.Persistance.MongoDocuments.TeacherMongo;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly IMongoCollection<TeacherDocument> _collection;

        public TeacherRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<TeacherDocument>("Teachers");
        }

        public async Task<Teacher?> GetByIdAsync(string id)
        {
            var filter = Builders<TeacherDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var document = await _collection.Find(filter).FirstOrDefaultAsync();
            return document?.ToDomain();
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            var documents = await _collection.Find(x => !x.IsDeleted).ToListAsync();
            return documents.ToDomainList();
        }

        public async Task<string> AddAsync(Teacher teacher)
        {
            var document = teacher.ToDocument();
            await _collection.InsertOneAsync(document);
            return document.Id.ToString();
        }

        public async Task<Teacher> UpdateAsync(Teacher teacher)
        {
            var filter = Builders<TeacherDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(teacher.Id));
            var document = teacher.ToDocument();
            var result = await _collection.ReplaceOneAsync(filter, document);
            if (result.MatchedCount == 0)
                return null;
            return document.ToDomain();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<TeacherDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<List<Teacher>> GetByIdsAsync(IReadOnlyCollection<string> ids)
        {
            var objectIds = ids
                .Where(id => ObjectId.TryParse(id, out _))
                .Select(ObjectId.Parse)
                .ToList();

            if (objectIds.Count == 0)
                return new List<Teacher>();

            var filter = Builders<TeacherDocument>.Filter.And(
                Builders<TeacherDocument>.Filter.In(x => x.Id, objectIds),
                Builders<TeacherDocument>.Filter.Eq(x => x.IsDeleted, false));

            var documents = await _collection.Find(filter).ToListAsync();
            return documents.ToDomainList();
        }

        public async Task<PagedResult<Teacher>> GetPagedAsync(QueryOptions options)
        {
            var filter = Builders<TeacherDocument>.Filter.Eq(x => x.IsDeleted, false);
            var pagedDoc = await QueryHelper.GetPagedResultAsync<TeacherDocument>(_collection, filter, options);
            return new PagedResult<Teacher>
            {
                Items = pagedDoc.Items.Select(TeacherMapper.ToDomain).ToList(),
                TotalCount = pagedDoc.TotalCount,
                Skip = pagedDoc.Skip,
                Limit = pagedDoc.Limit
            };
        }
    }
}
