using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.DynamicPages;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class DynamicPageConfiguration : IEntityTypeConfiguration<DynamicPage>
    {
        public void Configure(EntityTypeBuilder<DynamicPage> b)
        {
            b.ToTable("DynamicPages");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.Title).HasMaxLength(512).IsRequired();
            b.Property(x => x.Slug).HasMaxLength(512).IsRequired();
            b.Property(x => x.MetaTitle).HasMaxLength(512);
            b.Property(x => x.MetaDescription).HasMaxLength(1024);
            b.Property(x => x.MetaKeywords).HasMaxLength(1024);
            b.HasIndex(x => x.Slug);
            b.HasIndex(x => x.DisplayOrder);
            b.HasIndex(x => x.IsPublished);
        }
    }
}
