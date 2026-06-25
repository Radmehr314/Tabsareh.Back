using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Infrastructure.Persistance.Common;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly TabsarehDbContext _db;

        public CourseRepository(TabsarehDbContext db) => _db = db;

        public async Task<Course?> GetByIdAsync(string id)
            => await _db.Courses
                .Include(x => x.SampleVideos)
                .Include(x => x.PdfFiles)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Course>> GetAllAsync()
            => await _db.Courses.AsNoTracking().Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedAt).ToListAsync();

        public async Task<PagedResult<Course>> GetPagedAsync(QueryOptions options)
        {
            var query = _db.Courses.AsNoTracking().Where(x => !x.IsDeleted);
            return await QueryHelper.GetPagedResultAsync(query, options);
        }

        public async Task<string> AddAsync(Course course)
        {
            if (string.IsNullOrWhiteSpace(course.Id))
                course.SetId(Guid.NewGuid().ToString("N"));
            foreach (var video in course.SampleVideos)
                if (string.IsNullOrWhiteSpace(video.Id)) video.SetId(Guid.NewGuid().ToString("N"));
            foreach (var pdf in course.PdfFiles)
                if (string.IsNullOrWhiteSpace(pdf.Id)) pdf.SetId(Guid.NewGuid().ToString("N"));
            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
            return course.Id;
        }

        public async Task<Course> UpdateAsync(Course course)
        {
            var exists = await _db.Courses.AnyAsync(x => x.Id == course.Id);
            if (!exists) throw new InvalidOperationException("Course not found.");
            _db.Courses.Update(course);
            await _db.SaveChangesAsync();
            return course;
        }

        public async Task ReplaceAssetsAsync(string courseId, List<CourseSampleVideo> videos, List<CoursePdfFile> pdfs)
        {
            var oldVideos = await _db.CourseSampleVideos.Where(x => x.CourseId == courseId).ToListAsync();
            var oldPdfs = await _db.CoursePdfFiles.Where(x => x.CourseId == courseId).ToListAsync();
            _db.CourseSampleVideos.RemoveRange(oldVideos);
            _db.CoursePdfFiles.RemoveRange(oldPdfs);
            videos.ForEach(x => x.SetId(Guid.NewGuid().ToString("N")));
            pdfs.ForEach(x => x.SetId(Guid.NewGuid().ToString("N")));
            _db.CourseSampleVideos.AddRange(videos);
            _db.CoursePdfFiles.AddRange(pdfs);
            await _db.SaveChangesAsync();
        }
    }
}
