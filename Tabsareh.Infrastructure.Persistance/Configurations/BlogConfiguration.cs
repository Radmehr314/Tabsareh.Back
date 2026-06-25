using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Blogs;
using Tabsareh.Domain.Models.Categories;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> b)
        {
            b.ToTable("Blogs");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.Title).HasMaxLength(512).IsRequired();
            b.Property(x => x.Slug).HasMaxLength(512).IsRequired();
            b.Property(x => x.TitleImage).HasMaxLength(1024);
            b.Property(x => x.Excerpt).HasMaxLength(1024);
            b.Property(x => x.CategoryId).HasMaxLength(64);
            b.Property(x => x.MetaTitle).HasMaxLength(512);
            b.Property(x => x.MetaDescription).HasMaxLength(1024);
            b.Property(x => x.MetaKeywords).HasMaxLength(1024);
            b.HasIndex(x => x.Slug);
            b.HasIndex(x => x.CategoryId);
            b.HasIndex(x => x.IsPublished);
            b.HasOne<Category>()
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
