using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<string> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> ExistsByUserNameAsync(string userName, string? ignoreId = null);
        Task<PagedResult<User>> GetPagedAsync(QueryOptions options);
    }
}
