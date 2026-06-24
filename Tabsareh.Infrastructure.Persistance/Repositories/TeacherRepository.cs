using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Teachers;
using Tabsareh.Infrastructure.Persistance.Common;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly TabsarehDbContext _db;

        public TeacherRepository(TabsarehDbContext db)
        {
            _db = db;
        }

        public async Task<Teacher?> GetByIdAsync(string id)
            => await _db.Teachers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Teacher>> GetAllAsync()
            => await _db.Teachers.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();

        public async Task<string> AddAsync(Teacher teacher)
        {
            if (string.IsNullOrWhiteSpace(teacher.Id))
                teacher.SetId(Guid.NewGuid().ToString("N"));
            _db.Teachers.Add(teacher);
            await _db.SaveChangesAsync();
            return teacher.Id;
        }

        public async Task<Teacher> UpdateAsync(Teacher teacher)
        {
            var exists = await _db.Teachers.AnyAsync(x => x.Id == teacher.Id);
            if (!exists) return null;
            _db.Teachers.Update(teacher);
            await _db.SaveChangesAsync();
            return teacher;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var teacher = await _db.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (teacher is null) return false;
            _db.Teachers.Remove(teacher);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Teacher>> GetByIdsAsync(IReadOnlyCollection<string> ids)
        {
            if (ids is null || ids.Count == 0) return new List<Teacher>();
            return await _db.Teachers.AsNoTracking()
                .Where(x => ids.Contains(x.Id) && !x.IsDeleted).ToListAsync();
        }

        public async Task<PagedResult<Teacher>> GetPagedAsync(QueryOptions options)
        {
            var query = _db.Teachers.AsNoTracking().Where(x => !x.IsDeleted);
            return await QueryHelper.GetPagedResultAsync(query, options);
        }
    }
}
