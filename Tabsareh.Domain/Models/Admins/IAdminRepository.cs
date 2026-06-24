using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.Admins
{
    public interface IAdminRepository
    {
        Task<Admin?> GetByIdAsync(string id);
        Task<IEnumerable<Admin>> GetAllAsync();
        Task<string> AddAsync(Admin admin);
        Task<Admin> UpdateAsync(Admin admin);
        Task<bool> DeleteAsync(string id);

        Task<Admin?> GetByUserNameAsync(string userName);
        Task<bool> ExistsByUserNameAsync(string userName);
        Task<List<Admin>> GetByIdsAsync(IReadOnlyCollection<string> ids);
        Task<string?> BanUserAsync(string userName);
        Task<string?> UnbanUserAsync(string userName);
        Task<PagedResult<Admin>> GetPagedAsync(QueryOptions options);
    }
}
