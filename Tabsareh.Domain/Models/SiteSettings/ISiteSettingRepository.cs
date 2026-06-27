namespace Tabsareh.Domain.Models.SiteSettings
{
    public interface ISiteSettingRepository
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string value);
    }
}
