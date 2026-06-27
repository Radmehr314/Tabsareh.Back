using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Models.SiteSettings;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class SiteSettingRepository : ISiteSettingRepository
    {
        private readonly TabsarehDbContext _db;

        public SiteSettingRepository(TabsarehDbContext db) => _db = db;

        public async Task<string?> GetAsync(string key)
        {
            var setting = await _db.SiteSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Key == key);
            return setting?.Value;
        }

        public async Task SetAsync(string key, string value)
        {
            var setting = await _db.SiteSettings.FirstOrDefaultAsync(x => x.Key == key);
            if (setting is null)
            {
                _db.SiteSettings.Add(new SiteSetting { Key = key, Value = value });
            }
            else
            {
                setting.Value = value;
                _db.SiteSettings.Update(setting);
            }
            await _db.SaveChangesAsync();
        }
    }
}
