using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Categories;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> b)
        {
            b.ToTable("Categories");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.Name).HasMaxLength(256).IsRequired();
            b.Property(x => x.ParentId).HasMaxLength(64);
            b.HasIndex(x => x.ParentId);
            b.HasIndex(x => x.Name);
        }
    }
}
