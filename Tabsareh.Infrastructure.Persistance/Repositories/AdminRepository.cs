using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Admins;
using Tabsareh.Infrastructure.Persistance.Common;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly TabsarehDbContext _db;

        public AdminRepository(TabsarehDbContext db)
        {
            _db = db;
        }

        public async Task<Admin?> GetByIdAsync(string id)
            => await _db.Admins.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Admin>> GetAllAsync()
            => await _db.Admins.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();

        public async Task<string> AddAsync(Admin admin)
        {
            if (string.IsNullOrWhiteSpace(admin.Id))
                admin.SetId(Guid.NewGuid().ToString("N"));
            _db.Admins.Add(admin);
            await _db.SaveChangesAsync();
            return admin.Id;
        }

        public async Task<Admin> UpdateAsync(Admin admin)
        {
            var exists = await _db.Admins.AnyAsync(x => x.Id == admin.Id);
            if (!exists) return null;
            _db.Admins.Update(admin);
            await _db.SaveChangesAsync();
            return admin;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var admin = await _db.Admins.FirstOrDefaultAsync(x => x.Id == id);
            if (admin is null) return false;
            _db.Admins.Remove(admin);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Admin?> GetByUserNameAsync(string userName)
            => await _db.Admins.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == userName && !x.IsDeleted);

        public async Task<bool> ExistsByUserNameAsync(string userName)
            => await _db.Admins.AnyAsync(x => x.UserName == userName && !x.IsDeleted);

        public async Task<List<Admin>> GetByIdsAsync(IReadOnlyCollection<string> ids)
        {
            if (ids is null || ids.Count == 0) return new List<Admin>();
            return await _db.Admins.AsNoTracking()
                .Where(x => ids.Contains(x.Id) && !x.IsDeleted).ToListAsync();
        }

        public async Task<string?> BanUserAsync(string userName)
        {
            var admin = await _db.Admins.FirstOrDefaultAsync(x => x.UserName == userName);
            if (admin is null) return null;
            admin.Ban();
            await _db.SaveChangesAsync();
            return admin.Id;
        }

        public async Task<string?> UnbanUserAsync(string userName)
        {
            var admin = await _db.Admins.FirstOrDefaultAsync(x => x.UserName == userName);
            if (admin is null) return null;
            admin.UnBan();
            await _db.SaveChangesAsync();
            return admin.Id;
        }

        public async Task<PagedResult<Admin>> GetPagedAsync(QueryOptions options)
        {
            var query = _db.Admins.AsNoTracking().Where(x => !x.IsDeleted);
            return await QueryHelper.GetPagedResultAsync(query, options);
        }
    }
}
