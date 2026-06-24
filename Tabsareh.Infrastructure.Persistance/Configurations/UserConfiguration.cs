using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Users;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> b)
        {
            b.ToTable("Users");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.FirstName).HasMaxLength(256).IsRequired();
            b.Property(x => x.LastName).HasMaxLength(256).IsRequired();
            b.Property(x => x.UserName).HasMaxLength(256).IsRequired();
            b.Property(x => x.Phone).HasMaxLength(32).IsRequired();
            b.HasIndex(x => x.UserName);
        }
    }
}
