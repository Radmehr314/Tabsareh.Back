using MongoDB.Bson;
using MongoDB.Driver;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Infrastructure.Persistance.Common;
using Tabsareh.Infrastructure.Persistance.MongoDocuments.ContentOwnerMongo;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class ContentOwnerRepository : IContentOwnerRepository
    {
        private readonly IMongoCollection<ContentOwnerDocument> _collection;

        public ContentOwnerRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<ContentOwnerDocument>("ContentOwners");
        }

        public async Task<ContentOwner?> GetByIdAsync(string id)
        {
            var filter = Builders<ContentOwnerDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var document = await _collection.Find(filter).FirstOrDefaultAsync();
            return document?.ToDomain();
        }

        public async Task<IEnumerable<ContentOwner>> GetAllAsync()
        {
            var documents = await _collection.Find(x => !x.IsDeleted).ToListAsync();
            return documents.ToDomainList();
        }

        public async Task<string> AddAsync(ContentOwner contentOwner)
        {
            var document = contentOwner.ToDocument();
            await _collection.InsertOneAsync(document);
            return document.Id.ToString();
        }

        public async Task<ContentOwner> UpdateAsync(ContentOwner contentOwner)
        {
            var filter = Builders<ContentOwnerDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(contentOwner.Id));
            var document = contentOwner.ToDocument();
            var result = await _collection.ReplaceOneAsync(filter, document);
            if (result.MatchedCount == 0)
                return null;
            return document.ToDomain();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<ContentOwnerDocument>.Filter.Eq(x => x.Id, ObjectId.Parse(id));
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<ContentOwner?> GetByUserNameAsync(string userName)
        {
            var filter = Builders<ContentOwnerDocument>.Filter.And(
                Builders<ContentOwnerDocument>.Filter.Eq(x => x.UserName, userName),
                Builders<ContentOwnerDocument>.Filter.Eq(x => x.IsDeleted, false)
            );
            var document = await _collection.Find(filter).FirstOrDefaultAsync();
            return document?.ToDomain();
        }

        public async Task<bool> ExistsByUserNameAsync(string userName)
        {
            var filter = Builders<ContentOwnerDocument>.Filter.And(
                Builders<ContentOwnerDocument>.Filter.Eq(x => x.UserName, userName),
                Builders<ContentOwnerDocument>.Filter.Eq(x => x.IsDeleted, false)
            );
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task<string?> BanUserAsync(string userName)
        {
            var filter = Builders<ContentOwnerDocument>.Filter.Eq(x => x.UserName, userName);
            var update = Builders<ContentOwnerDocument>.Update.Set(x => x.IsBan, true);
            await _collection.UpdateOneAsync(filter, update);
            var user = await _collection.Find(filter).FirstOrDefaultAsync();
            return user?.Id.ToString();
        }

        public async Task<string?> UnbanUserAsync(string userName)
        {
            var filter = Builders<ContentOwnerDocument>.Filter.Eq(x => x.UserName, userName);
            var update = Builders<ContentOwnerDocument>.Update.Set(x => x.IsBan, false);
            await _collection.UpdateOneAsync(filter, update);
            var user = await _collection.Find(filter).FirstOrDefaultAsync();
            return user?.Id.ToString();
        }

        public async Task<PagedResult<ContentOwner>> GetPagedAsync(QueryOptions options)
        {
            var filter = Builders<ContentOwnerDocument>.Filter.Eq(x => x.IsDeleted, false);
            var pagedDoc = await QueryHelper.GetPagedResultAsync<ContentOwnerDocument>(_collection, filter, options);
            return new PagedResult<ContentOwner>
            {
                Items = pagedDoc.Items.Select(ContentOwnerMapper.ToDomain).ToList(),
                TotalCount = pagedDoc.TotalCount,
                Skip = pagedDoc.Skip,
                Limit = pagedDoc.Limit
            };
        }
    }
}
