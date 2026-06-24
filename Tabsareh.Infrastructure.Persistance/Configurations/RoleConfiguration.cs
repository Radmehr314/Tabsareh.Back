using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Roles;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> b)
        {
            b.ToTable("Roles");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.Name).HasMaxLength(256).IsRequired();

            b.Property(x => x.Permissions)
                .HasConversion(StringListConverter.Converter)
                .Metadata.SetValueComparer(StringListConverter.Comparer);
        }
    }
}
