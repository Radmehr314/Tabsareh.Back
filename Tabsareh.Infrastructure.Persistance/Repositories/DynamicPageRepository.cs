using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.DynamicPages;
using Tabsareh.Infrastructure.Persistance.Common;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class DynamicPageRepository : IDynamicPageRepository
    {
        private readonly TabsarehDbContext _db;

        public DynamicPageRepository(TabsarehDbContext db)
        {
            _db = db;
        }

        public async Task<DynamicPage?> GetByIdAsync(string id)
            => await _db.DynamicPages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<DynamicPage?> GetBySlugAsync(string slug, bool onlyPublished = false)
            => await _db.DynamicPages.AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Slug == slug &&
                    !x.IsDeleted &&
                    (!onlyPublished || x.IsPublished));

        public async Task<IEnumerable<DynamicPage>> GetAllAsync(bool onlyPublished = false)
            => await _db.DynamicPages.AsNoTracking()
                .Where(x => !x.IsDeleted && (!onlyPublished || x.IsPublished))
                .OrderBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync();

        public async Task<string> AddAsync(DynamicPage page)
        {
            if (string.IsNullOrWhiteSpace(page.Id))
                page.SetId(Guid.NewGuid().ToString("N"));
            _db.DynamicPages.Add(page);
            await _db.SaveChangesAsync();
            return page.Id;
        }

        public async Task<DynamicPage> UpdateAsync(DynamicPage page)
        {
            var exists = await _db.DynamicPages.AnyAsync(x => x.Id == page.Id);
            if (!exists) return null;
            _db.DynamicPages.Update(page);
            await _db.SaveChangesAsync();
            return page;
        }

        public async Task<bool> ExistsBySlugAsync(string slug, string? ignoreId = null)
        {
            return await _db.DynamicPages.AnyAsync(x =>
                !x.IsDeleted &&
                x.Slug == slug &&
                (ignoreId == null || x.Id != ignoreId));
        }

        public async Task<PagedResult<DynamicPage>> GetPagedAsync(QueryOptions options)
        {
            var query = _db.DynamicPages.AsNoTracking().Where(x => !x.IsDeleted);
            return await QueryHelper.GetPagedResultAsync(query, options);
        }
    }
}
