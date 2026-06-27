using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.SiteSettings;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
    {
        public void Configure(EntityTypeBuilder<SiteSetting> b)
        {
            b.ToTable("SiteSettings");
            b.HasKey(x => x.Key);
            b.Property(x => x.Key).HasMaxLength(128);
            b.Property(x => x.Value).HasMaxLength(1024).IsRequired();
        }
    }
}
