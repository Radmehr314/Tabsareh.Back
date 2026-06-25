using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.Courses
{
    public interface ICourseChapterRepository
    {
        Task<CourseChapter?> GetByIdAsync(string id);
        Task<IEnumerable<CourseChapter>> GetByCourseIdAsync(string courseId);
        Task<PagedResult<CourseChapter>> GetPagedAsync(QueryOptions options);
        Task<string> AddAsync(CourseChapter chapter);
        Task<CourseChapter> UpdateAsync(CourseChapter chapter);
        Task ReplaceVideosAsync(string chapterId, List<CourseChapterVideo> videos);
    }
}
