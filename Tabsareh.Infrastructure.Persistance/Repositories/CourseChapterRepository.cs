using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Infrastructure.Persistance.Common;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class CourseChapterRepository : ICourseChapterRepository
    {
        private readonly TabsarehDbContext _db;

        public CourseChapterRepository(TabsarehDbContext db) => _db = db;

        public async Task<CourseChapter?> GetByIdAsync(string id)
            => await _db.CourseChapters
                .Include(x => x.Videos)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<CourseChapter>> GetByCourseIdAsync(string courseId)
            => await _db.CourseChapters.AsNoTracking()
                .Include(x => x.Videos)
                .Where(x => x.CourseId == courseId && !x.IsDeleted)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.CreatedAt)
                .ToListAsync();

        public async Task<PagedResult<CourseChapter>> GetPagedAsync(QueryOptions options)
        {
            var query = _db.CourseChapters.AsNoTracking().Where(x => !x.IsDeleted);
            return await QueryHelper.GetPagedResultAsync(query, options);
        }

        public async Task<string> AddAsync(CourseChapter chapter)
        {
            if (string.IsNullOrWhiteSpace(chapter.Id))
                chapter.SetId(Guid.NewGuid().ToString("N"));
            foreach (var video in chapter.Videos)
                if (string.IsNullOrWhiteSpace(video.Id)) video.SetId(Guid.NewGuid().ToString("N"));
            _db.CourseChapters.Add(chapter);
            await _db.SaveChangesAsync();
            return chapter.Id;
        }

        public async Task<CourseChapter> UpdateAsync(CourseChapter chapter)
        {
            var exists = await _db.CourseChapters.AnyAsync(x => x.Id == chapter.Id);
            if (!exists) throw new InvalidOperationException("Course chapter not found.");
            _db.CourseChapters.Update(chapter);
            await _db.SaveChangesAsync();
            return chapter;
        }

        public async Task ReplaceVideosAsync(string chapterId, List<CourseChapterVideo> videos)
        {
            var oldVideos = await _db.CourseChapterVideos.Where(x => x.CourseChapterId == chapterId).ToListAsync();
            _db.CourseChapterVideos.RemoveRange(oldVideos);
            videos.ForEach(x => x.SetId(Guid.NewGuid().ToString("N")));
            _db.CourseChapterVideos.AddRange(videos);
            await _db.SaveChangesAsync();
        }
    }
}
