using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.Roles
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(string id);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<PagedResult<Role>> GetPagedAsync(QueryOptions options);
        Task<string> AddAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
