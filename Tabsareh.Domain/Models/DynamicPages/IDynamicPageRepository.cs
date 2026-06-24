using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.DynamicPages
{
    public interface IDynamicPageRepository
    {
        Task<DynamicPage?> GetByIdAsync(string id);
        Task<DynamicPage?> GetBySlugAsync(string slug, bool onlyPublished = false);
        Task<IEnumerable<DynamicPage>> GetAllAsync(bool onlyPublished = false);
        Task<string> AddAsync(DynamicPage page);
        Task<DynamicPage> UpdateAsync(DynamicPage page);
        Task<bool> ExistsBySlugAsync(string slug, string? ignoreId = null);
        Task<PagedResult<DynamicPage>> GetPagedAsync(QueryOptions options);
    }
}
