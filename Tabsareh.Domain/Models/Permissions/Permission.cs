namespace Tabsareh.Domain.Models.Permissions
{
    public static class Permission
    {
        public const string ViewDashboard = "view_dashboard";
        public const string ManageAdmins = "manage_admins";
        public const string ManageRoles = "manage_roles";
        public const string ManageTeachers = "manage_teachers";
        public const string ManageContentOwners = "manage_content_owners";
        public const string ManageCategories = "manage_categories";
        public const string ManageUsers = "manage_users";
        public const string ManageBlogs = "manage_blogs";
        public const string ManageDynamicPages = "manage_dynamic_pages";
        public const string ManageCourses = "manage_courses";
        public const string ManageCourseChapters = "manage_course_chapters";
        public const string ManageDiscounts = "manage_discounts";
        public const string ManageOrders = "manage_orders";
        public const string ManageCardToCard = "manage_card_to_card";
        public const string ManageComments = "manage_comments";

        public static readonly IReadOnlyList<string> All = new[]
        {
            ViewDashboard,
            ManageAdmins,
            ManageRoles,
            ManageTeachers,
            ManageContentOwners,
            ManageCategories,
            ManageUsers,
            ManageBlogs,
            ManageDynamicPages,
            ManageCourses,
            ManageCourseChapters,
            ManageDiscounts,
            ManageOrders,
            ManageCardToCard,
            ManageComments,
        };
    }
}
