using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.ContentOwners
{
    public interface IContentOwnerRepository
    {
        Task<ContentOwner?> GetByIdAsync(string id);
        Task<IEnumerable<ContentOwner>> GetAllAsync();
        Task<string> AddAsync(ContentOwner contentOwner);
        Task<ContentOwner> UpdateAsync(ContentOwner contentOwner);
        Task<bool> DeleteAsync(string id);

        Task<ContentOwner?> GetByUserNameAsync(string userName);
        Task<bool> ExistsByUserNameAsync(string userName);
        Task<string?> BanUserAsync(string userName);
        Task<string?> UnbanUserAsync(string userName);
        Task<PagedResult<ContentOwner>> GetPagedAsync(QueryOptions options);
    }
}
