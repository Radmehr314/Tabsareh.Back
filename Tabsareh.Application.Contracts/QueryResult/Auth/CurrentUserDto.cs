using Tabsareh.Application.Contracts.QueryResult.ContentOwner;

namespace Tabsareh.Application.Contracts.QueryResult.Auth
{
    /// <summary>
    /// اطلاعات کاربر لاگین‌شده (ادمین یا صاحب اثر) برای نمایش در پنل.
    /// </summary>
    public class CurrentUserDto
    {
        public string Id { get; set; }

        /// <summary>admin یا content_owner</summary>
        public string Role { get; set; }

        /// <summary>نام نمایشی برای پنل</summary>
        public string FullName { get; set; }

        public string UserName { get; set; }

        // ----- مخصوص ادمین -----
        public string? Phone { get; set; }
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public List<string> Permissions { get; set; } = new();

        // ----- مخصوص صاحب اثر -----
        public List<string> TeacherIds { get; set; } = new();
        public List<TeacherBriefDto> Teachers { get; set; } = new();
    }
}
