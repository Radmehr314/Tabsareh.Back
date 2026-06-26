using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.CourseComments
{
    public class CourseCommentFilter
    {
        public string? CourseId { get; set; }
        public bool? IsApproved { get; set; }
    }

    public class CourseCommentListItem
    {
        public CourseComment Comment { get; set; } = null!;
        public string CourseTitle { get; set; } = string.Empty;
    }

    public interface ICourseCommentRepository
    {
        Task<string> AddAsync(CourseComment comment);
        Task<CourseComment?> GetByIdAsync(string id);
        Task<PagedResult<CourseCommentListItem>> GetPagedAsync(QueryOptions options, CourseCommentFilter filter);
        Task<List<CourseComment>> GetApprovedByCourseIdAsync(string courseId);
        Task<double?> GetAverageRatingByCourseIdAsync(string courseId);
        Task UpdateAsync(CourseComment comment);
    }
}
