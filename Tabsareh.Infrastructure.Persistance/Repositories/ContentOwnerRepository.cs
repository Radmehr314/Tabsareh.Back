using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Infrastructure.Persistance.Common;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class ContentOwnerRepository : IContentOwnerRepository
    {
        private readonly TabsarehDbContext _db;

        public ContentOwnerRepository(TabsarehDbContext db)
        {
            _db = db;
        }

        public async Task<ContentOwner?> GetByIdAsync(string id)
            => await _db.ContentOwners.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<ContentOwner>> GetAllAsync()
            => await _db.ContentOwners.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();

        public async Task<string> AddAsync(ContentOwner contentOwner)
        {
            if (string.IsNullOrWhiteSpace(contentOwner.Id))
                contentOwner.SetId(Guid.NewGuid().ToString("N"));
            _db.ContentOwners.Add(contentOwner);
            await _db.SaveChangesAsync();
            return contentOwner.Id;
        }

        public async Task<ContentOwner> UpdateAsync(ContentOwner contentOwner)
        {
            var exists = await _db.ContentOwners.AnyAsync(x => x.Id == contentOwner.Id);
            if (!exists) return null;
            _db.ContentOwners.Update(contentOwner);
            await _db.SaveChangesAsync();
            return contentOwner;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var owner = await _db.ContentOwners.FirstOrDefaultAsync(x => x.Id == id);
            if (owner is null) return false;
            _db.ContentOwners.Remove(owner);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<ContentOwner?> GetByUserNameAsync(string userName)
            => await _db.ContentOwners.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == userName && !x.IsDeleted);

        public async Task<bool> ExistsByUserNameAsync(string userName)
            => await _db.ContentOwners.AnyAsync(x => x.UserName == userName && !x.IsDeleted);

        public async Task<string?> BanUserAsync(string userName)
        {
            var owner = await _db.ContentOwners.FirstOrDefaultAsync(x => x.UserName == userName);
            if (owner is null) return null;
            owner.Ban();
            await _db.SaveChangesAsync();
            return owner.Id;
        }

        public async Task<string?> UnbanUserAsync(string userName)
        {
            var owner = await _db.ContentOwners.FirstOrDefaultAsync(x => x.UserName == userName);
            if (owner is null) return null;
            owner.UnBan();
            await _db.SaveChangesAsync();
            return owner.Id;
        }

        public async Task<PagedResult<ContentOwner>> GetPagedAsync(QueryOptions options)
        {
            var query = _db.ContentOwners.AsNoTracking().Where(x => !x.IsDeleted);
            return await QueryHelper.GetPagedResultAsync(query, options);
        }
    }
}
