using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.Categories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(string id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<string> AddAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> ExistsByNameAsync(string name, string? parentId, string? ignoreId = null);
        Task<bool> HasChildrenAsync(string id);
        Task<PagedResult<Category>> GetPagedAsync(QueryOptions options);
    }
}
