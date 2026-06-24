namespace Tabsareh.Domain.Models.Permissions
{
    public static class Permission
    {
        public const string ViewDashboard = "view_dashboard";
        public const string ManageAdmins = "manage_admins";
        public const string ManageRoles = "manage_roles";
        public const string ManageTeachers = "manage_teachers";
        public const string ManageContentOwners = "manage_content_owners";

        public static readonly IReadOnlyList<string> All = new[]
        {
            ViewDashboard,
            ManageAdmins,
            ManageRoles,
            ManageTeachers,
            ManageContentOwners,
        };
    }
}
