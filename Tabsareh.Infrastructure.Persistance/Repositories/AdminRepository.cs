using MongoDB.Bson;
using MongoDB.Driver;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Admins;
using Tabsareh.Infrastructure.Persistance.Common;
using Tabsareh.Infrastructure.Persistance.MongoDocuments.AdminMongo;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IMongoCollection<AdminDocument> _collection;

        public AdminRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<AdminDocument>("Admins");
        }

        public async Task<Admin?> GetByIdAsync(string id)
        {
            var filter = Builders<AdminDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var document = await _collection.Find(filter).FirstOrDefaultAsync();
            return document?.ToDomain();
        }

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            var documents = await _collection.Find(x => !x.IsDeleted).ToListAsync();
            return documents.ToDomainList();
        }

        public async Task<string> AddAsync(Admin admin)
        {
            var document = admin.ToDocument();
            await _collection.InsertOneAsync(document);
            return document.Id.ToString();
        }

        public async Task<Admin> UpdateAsync(Admin admin)
        {
            var filter = Builders<AdminDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(admin.Id));
            var document = admin.ToDocument();

            var result = await _collection.ReplaceOneAsync(filter, document);

            if (result.MatchedCount == 0)
                return null;

            return document.ToDomain();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<AdminDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<Admin?> GetByUserNameAsync(string userName)
        {
            var filter = Builders<AdminDocument>.Filter.And(
                Builders<AdminDocument>.Filter.Eq(x => x.UserName, userName),
                Builders<AdminDocument>.Filter.Eq(x => x.IsDeleted, false)
            );
            var document = await _collection.Find(filter).FirstOrDefaultAsync();
            return document?.ToDomain();
        }

        public async Task<bool> ExistsByUserNameAsync(string userName)
        {
            var filter = Builders<AdminDocument>.Filter.And(
                Builders<AdminDocument>.Filter.Eq(x => x.UserName, userName),
                Builders<AdminDocument>.Filter.Eq(x => x.IsDeleted, false)
            );
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task<List<Admin>> GetByIdsAsync(IReadOnlyCollection<string> ids)
        {
            var objectIds = ids
                .Where(id => ObjectId.TryParse(id, out _))
                .Select(ObjectId.Parse)
                .ToList();

            if (objectIds.Count == 0)
            {
                return new List<Admin>();
            }

            var filter = Builders<AdminDocument>.Filter.And(
                Builders<AdminDocument>.Filter.In(x => x.Id, objectIds),
                Builders<AdminDocument>.Filter.Eq(x => x.IsDeleted, false));

            var documents = await _collection.Find(filter).ToListAsync();
            return documents.ToDomainList().ToList();
        }

        public async Task<string?> BanUserAsync(string userName)
        {
            var filter = Builders<AdminDocument>.Filter.Eq(x => x.UserName, userName);

            var update = Builders<AdminDocument>.Update
                .Set(x => x.IsBan, true);

            var result = await _collection.UpdateOneAsync(filter, update);

            var user = await _collection.Find(filter).FirstOrDefaultAsync();

            return user?.Id.ToString();
        }

        public async Task<string?> UnbanUserAsync(string userName)
        {
            var filter = Builders<AdminDocument>.Filter.Eq(x => x.UserName, userName);

            var update = Builders<AdminDocument>.Update
                .Set(x => x.IsBan, false);

            var result = await _collection.UpdateOneAsync(filter, update);

            var user = await _collection.Find(filter).FirstOrDefaultAsync();

            return user?.Id.ToString();
        }

        public async Task<PagedResult<Admin>> GetPagedAsync(QueryOptions options)
        {
            var filter = Builders<AdminDocument>.Filter.Eq(x => x.IsDeleted, false);
            var pagedDoc = await QueryHelper.GetPagedResultAsync<AdminDocument>(_collection, filter, options);
            return new PagedResult<Admin>
            {
                Items = pagedDoc.Items.Select(AdminMapper.ToDomain).ToList(),
                TotalCount = pagedDoc.TotalCount,
                Skip = pagedDoc.Skip,
                Limit = pagedDoc.Limit
            };
        }
    }
}
