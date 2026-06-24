using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Blogs;
using Tabsareh.Infrastructure.Persistance.Common;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly TabsarehDbContext _db;

        public BlogRepository(TabsarehDbContext db)
        {
            _db = db;
        }

        public async Task<Blog?> GetByIdAsync(string id)
            => await _db.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Blog?> GetBySlugAsync(string slug)
            => await _db.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug && !x.IsDeleted);

        public async Task<IEnumerable<Blog>> GetAllAsync()
            => await _db.Blogs.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();

        public async Task<string> AddAsync(Blog blog)
        {
            if (string.IsNullOrWhiteSpace(blog.Id))
                blog.SetId(Guid.NewGuid().ToString("N"));
            _db.Blogs.Add(blog);
            await _db.SaveChangesAsync();
            return blog.Id;
        }

        public async Task<Blog> UpdateAsync(Blog blog)
        {
            var exists = await _db.Blogs.AnyAsync(x => x.Id == blog.Id);
            if (!exists) return null;
            _db.Blogs.Update(blog);
            await _db.SaveChangesAsync();
            return blog;
        }

        public async Task<bool> ExistsBySlugAsync(string slug, string? ignoreId = null)
        {
            return await _db.Blogs.AnyAsync(x =>
                !x.IsDeleted &&
                x.Slug == slug &&
                (ignoreId == null || x.Id != ignoreId));
        }

        public async Task<PagedResult<Blog>> GetPagedAsync(QueryOptions options)
        {
            var query = _db.Blogs.AsNoTracking().Where(x => !x.IsDeleted);
            return await QueryHelper.GetPagedResultAsync(query, options);
        }
    }
}
