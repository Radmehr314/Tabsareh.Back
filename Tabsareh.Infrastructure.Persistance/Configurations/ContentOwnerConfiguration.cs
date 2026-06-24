using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.ContentOwners;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class ContentOwnerConfiguration : IEntityTypeConfiguration<ContentOwner>
    {
        public void Configure(EntityTypeBuilder<ContentOwner> b)
        {
            b.ToTable("ContentOwners");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.Name).HasMaxLength(256);
            b.Property(x => x.UserName).HasMaxLength(256).IsRequired();
            b.HasIndex(x => x.UserName);
        }
    }
}
