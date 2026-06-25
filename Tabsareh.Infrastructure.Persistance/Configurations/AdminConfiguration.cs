using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Admins;
using Tabsareh.Domain.Models.Roles;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> b)
        {
            b.ToTable("Admins");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.UserName).HasMaxLength(256).IsRequired();
            b.HasIndex(x => x.UserName);
            b.Property(x => x.FirstName).HasMaxLength(256);
            b.Property(x => x.LastName).HasMaxLength(256);
            b.Property(x => x.Phone).HasMaxLength(32);
            b.Property(x => x.RoleId).HasMaxLength(64);
            b.HasOne<Role>()
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
