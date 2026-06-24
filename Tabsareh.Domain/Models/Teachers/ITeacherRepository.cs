using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.Teachers
{
    public interface ITeacherRepository
    {
        Task<Teacher?> GetByIdAsync(string id);
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<string> AddAsync(Teacher teacher);
        Task<Teacher> UpdateAsync(Teacher teacher);
        Task<bool> DeleteAsync(string id);
        Task<List<Teacher>> GetByIdsAsync(IReadOnlyCollection<string> ids);
        Task<PagedResult<Teacher>> GetPagedAsync(QueryOptions options);
    }
}
