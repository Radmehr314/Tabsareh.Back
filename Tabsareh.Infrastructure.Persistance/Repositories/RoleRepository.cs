using MongoDB.Bson;
using MongoDB.Driver;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Roles;
using Tabsareh.Infrastructure.Persistance.Common;
using Tabsareh.Infrastructure.Persistance.MongoDocuments.RoleMongo;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IMongoCollection<RoleDocument> _collection;

        public RoleRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<RoleDocument>("Roles");
        }

        public async Task<Role?> GetByIdAsync(string id)
        {
            var filter = Builders<RoleDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var document = await _collection.Find(filter).FirstOrDefaultAsync();
            return document?.ToDomain();
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var documents = await _collection.Find(_ => true).ToListAsync();
            return documents.Select(d => d.ToDomain()).ToList();
        }

        public async Task<PagedResult<Role>> GetPagedAsync(QueryOptions options)
        {
            var filter = Builders<RoleDocument>.Filter.Empty;
            var pagedDoc = await QueryHelper.GetPagedResultAsync<RoleDocument>(_collection, filter, options);
            return new PagedResult<Role>
            {
                Items = pagedDoc.Items.Select(d => d.ToDomain()).ToList(),
                TotalCount = pagedDoc.TotalCount,
                Skip = pagedDoc.Skip,
                Limit = pagedDoc.Limit
            };
        }

        public async Task<string> AddAsync(Role role)
        {
            var document = role.ToDocument();
            await _collection.InsertOneAsync(document);
            return document.Id.ToString();
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            var filter = Builders<RoleDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(role.Id));
            var document = role.ToDocument();
            var result = await _collection.ReplaceOneAsync(filter, document);
            if (result.MatchedCount == 0)
                return null;
            return document.ToDomain();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<RoleDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            var filter = Builders<RoleDocument>.Filter.Eq(x => x.Name, name);
            return await _collection.CountDocumentsAsync(filter) > 0;
        }
    }
}
