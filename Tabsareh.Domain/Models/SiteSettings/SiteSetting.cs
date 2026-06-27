namespace Tabsareh.Domain.Models.SiteSettings
{
    public class SiteSetting
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public static class SiteSettingKeys
    {
        public const string LicensePrice = "license_price";
    }
}
