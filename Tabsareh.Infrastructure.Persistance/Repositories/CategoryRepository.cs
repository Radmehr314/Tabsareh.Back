using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Infrastructure.Persistance.Common;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TabsarehDbContext _db;

        public CategoryRepository(TabsarehDbContext db)
        {
            _db = db;
        }

        public async Task<Category?> GetByIdAsync(string id)
            => await _db.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Category>> GetAllAsync()
            => await _db.Categories.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();

        public async Task<string> AddAsync(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Id))
                category.SetId(Guid.NewGuid().ToString("N"));
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return category.Id;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var exists = await _db.Categories.AnyAsync(x => x.Id == category.Id);
            if (!exists) return null;
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<bool> ExistsByNameAsync(string name, string? parentId, string? ignoreId = null)
        {
            return await _db.Categories.AnyAsync(x =>
                !x.IsDeleted &&
                x.Name == name &&
                x.ParentId == parentId &&
                (ignoreId == null || x.Id != ignoreId));
        }

        public async Task<bool> HasChildrenAsync(string id)
            => await _db.Categories.AnyAsync(x => !x.IsDeleted && x.ParentId == id);

        public async Task<PagedResult<Category>> GetPagedAsync(QueryOptions options)
        {
            var query = _db.Categories.AsNoTracking().Where(x => !x.IsDeleted);
            return await QueryHelper.GetPagedResultAsync(query, options);
        }
    }
}
