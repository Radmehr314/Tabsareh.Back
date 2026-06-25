using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.Courses
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(string id);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<PagedResult<Course>> GetPagedAsync(QueryOptions options);
        Task<string> AddAsync(Course course);
        Task<Course> UpdateAsync(Course course);
        Task ReplaceAssetsAsync(string courseId, List<CourseSampleVideo> videos, List<CoursePdfFile> pdfs);
    }
}
