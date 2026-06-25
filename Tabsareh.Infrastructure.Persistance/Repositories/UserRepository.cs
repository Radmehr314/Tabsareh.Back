using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Users;
using Tabsareh.Infrastructure.Persistance.Common;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TabsarehDbContext _db;

        public UserRepository(TabsarehDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetByIdAsync(string id)
            => await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<User?> GetByPhoneAsync(string phone)
            => await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Phone == phone && !x.IsDeleted);

        public async Task<IEnumerable<User>> GetAllAsync()
            => await _db.Users.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();

        public async Task<string> AddAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Id))
                user.SetId(Guid.NewGuid().ToString("N"));
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var exists = await _db.Users.AnyAsync(x => x.Id == user.Id);
            if (!exists) return null;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<bool> ExistsByUserNameAsync(string userName, string? ignoreId = null)
        {
            return await _db.Users.AnyAsync(x =>
                !x.IsDeleted &&
                x.UserName == userName &&
                (ignoreId == null || x.Id != ignoreId));
        }

        public async Task<bool> ExistsByPhoneAsync(string phone, string? ignoreId = null)
        {
            return await _db.Users.AnyAsync(x =>
                !x.IsDeleted &&
                x.Phone == phone &&
                (ignoreId == null || x.Id != ignoreId));
        }

        public async Task<PagedResult<User>> GetPagedAsync(QueryOptions options)
        {
            var query = _db.Users.AsNoTracking().Where(x => !x.IsDeleted);
            return await QueryHelper.GetPagedResultAsync(query, options);
        }
    }
}
