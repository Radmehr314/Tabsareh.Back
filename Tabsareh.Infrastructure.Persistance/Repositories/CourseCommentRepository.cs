using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.CourseComments;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class CourseCommentRepository : ICourseCommentRepository
    {
        private readonly TabsarehDbContext _db;

        public CourseCommentRepository(TabsarehDbContext db) => _db = db;

        public async Task<string> AddAsync(CourseComment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Id))
                comment.SetId(Guid.NewGuid().ToString("N"));
            _db.CourseComments.Add(comment);
            await _db.SaveChangesAsync();
            return comment.Id;
        }

        public async Task<CourseComment?> GetByIdAsync(string id)
            => await _db.CourseComments.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<PagedResult<CourseCommentListItem>> GetPagedAsync(QueryOptions options, CourseCommentFilter filter)
        {
            var query =
                from comment in _db.CourseComments.AsNoTracking()
                join course in _db.Courses.AsNoTracking() on comment.CourseId equals course.Id
                where !comment.IsDeleted
                select new CourseCommentListItem
                {
                    Comment = comment,
                    CourseTitle = course.Title,
                };

            if (!string.IsNullOrWhiteSpace(filter.CourseId))
                query = query.Where(x => x.Comment.CourseId == filter.CourseId);

            if (filter.IsApproved.HasValue)
                query = query.Where(x => x.Comment.IsApproved == filter.IsApproved.Value);

            var totalCount = await query.LongCountAsync();
            var items = await query
                .OrderByDescending(x => x.Comment.CreatedAt)
                .Skip(options.Skip)
                .Take(options.Limit)
                .ToListAsync();

            return new PagedResult<CourseCommentListItem>
            {
                Items = items,
                TotalCount = totalCount,
                Skip = options.Skip,
                Limit = options.Limit,
            };
        }

        public async Task<List<CourseComment>> GetApprovedByCourseIdAsync(string courseId)
            => await _db.CourseComments.AsNoTracking()
                .Where(x => x.CourseId == courseId && x.IsApproved && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

        public async Task<double?> GetAverageRatingByCourseIdAsync(string courseId)
            => await _db.CourseComments.AsNoTracking()
                .Where(x => x.CourseId == courseId && x.IsApproved && !x.IsDeleted)
                .AverageAsync(x => (double?)x.Rating);

        public async Task UpdateAsync(CourseComment comment)
        {
            _db.CourseComments.Update(comment);
            await _db.SaveChangesAsync();
        }
    }
}
