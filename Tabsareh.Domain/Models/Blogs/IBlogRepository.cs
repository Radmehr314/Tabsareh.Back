using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.Blogs
{
    public interface IBlogRepository
    {
        Task<Blog?> GetByIdAsync(string id);
        Task<Blog?> GetBySlugAsync(string slug);
        Task<IEnumerable<Blog>> GetAllAsync();
        Task<string> AddAsync(Blog blog);
        Task<Blog> UpdateAsync(Blog blog);
        Task<bool> ExistsBySlugAsync(string slug, string? ignoreId = null);
        Task<PagedResult<Blog>> GetPagedAsync(QueryOptions options);
    }
}
