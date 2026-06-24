using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Models.Admins;
using Tabsareh.Domain.Models.Blogs;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.DynamicPages;
using Tabsareh.Domain.Models.Roles;
using Tabsareh.Domain.Models.Teachers;
using Tabsareh.Domain.Models.Users;

namespace Tabsareh.Infrastructure.Persistance
{
    public class TabsarehDbContext : DbContext
    {
        public TabsarehDbContext(DbContextOptions<TabsarehDbContext> options) : base(options) { }

        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<ContentOwner> ContentOwners => Set<ContentOwner>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Blog> Blogs => Set<Blog>();
        public DbSet<DynamicPage> DynamicPages => Set<DynamicPage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TabsarehDbContext).Assembly);
        }
    }
}
