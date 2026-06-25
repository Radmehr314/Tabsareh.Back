using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Models.Admins;
using Tabsareh.Domain.Models.Auth;
using Tabsareh.Domain.Models.Blogs;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Domain.Models.DynamicPages;
using Tabsareh.Domain.Models.Discounts;
using Tabsareh.Domain.Models.Orders;
using Tabsareh.Domain.Models.Roles;
using Tabsareh.Domain.Models.Teachers;
using Tabsareh.Domain.Models.Users;

namespace Tabsareh.Infrastructure.Persistance
{
    public class TabsarehDbContext : DbContext
    {
        public TabsarehDbContext(DbContextOptions<TabsarehDbContext> options) : base(options) { }

        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<SmsOtp> SmsOtps => Set<SmsOtp>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<ContentOwner> ContentOwners => Set<ContentOwner>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Blog> Blogs => Set<Blog>();
        public DbSet<DynamicPage> DynamicPages => Set<DynamicPage>();
        public DbSet<DiscountCode> DiscountCodes => Set<DiscountCode>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<CourseSampleVideo> CourseSampleVideos => Set<CourseSampleVideo>();
        public DbSet<CoursePdfFile> CoursePdfFiles => Set<CoursePdfFile>();
        public DbSet<CourseChapter> CourseChapters => Set<CourseChapter>();
        public DbSet<CourseChapterVideo> CourseChapterVideos => Set<CourseChapterVideo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TabsarehDbContext).Assembly);
        }
    }
}
