using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Roles;
using Tabsareh.Infrastructure.Persistance.Common;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly TabsarehDbContext _db;

        public RoleRepository(TabsarehDbContext db)
        {
            _db = db;
        }

        public async Task<Role?> GetByIdAsync(string id)
            => await _db.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Role>> GetAllAsync()
            => await _db.Roles.AsNoTracking().ToListAsync();

        public async Task<PagedResult<Role>> GetPagedAsync(QueryOptions options)
        {
            var query = _db.Roles.AsNoTracking();
            return await QueryHelper.GetPagedResultAsync(query, options);
        }

        public async Task<string> AddAsync(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.Id))
                role.SetId(Guid.NewGuid().ToString("N"));
            _db.Roles.Add(role);
            await _db.SaveChangesAsync();
            return role.Id;
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            var exists = await _db.Roles.AnyAsync(x => x.Id == role.Id);
            if (!exists) return null;
            _db.Roles.Update(role);
            await _db.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role is null) return false;
            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByNameAsync(string name)
            => await _db.Roles.AnyAsync(x => x.Name == name);
    }
}
