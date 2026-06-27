using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Categories;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Domain.Models.Teachers;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> b)
        {
            b.ToTable("Courses");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.Title).HasMaxLength(512).IsRequired();
            b.Property(x => x.BannerImage).HasMaxLength(1024);
            b.Property(x => x.CategoryId).HasMaxLength(64);
            b.Property(x => x.TeacherId).HasMaxLength(64).IsRequired();
            b.Property(x => x.ContentOwnerId).HasMaxLength(64).IsRequired();
            b.Property(x => x.Price).HasColumnType("decimal(18,2)");
            b.Property(x => x.ContentOwnerSharePercent).HasColumnType("decimal(5,2)");
            b.Property(x => x.DiscountPercent).HasColumnType("decimal(5,2)");
            b.HasIndex(x => x.CategoryId);
            b.HasIndex(x => x.TeacherId);
            b.HasIndex(x => x.ContentOwnerId);
            b.HasIndex(x => x.IsActive);
            b.Property(x => x.AverageRating).HasColumnType("float");
            b.Property(x => x.CommentCount).HasDefaultValue(0);

            b.HasOne<Category>().WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne<Teacher>().WithMany().HasForeignKey(x => x.TeacherId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne<ContentOwner>().WithMany().HasForeignKey(x => x.ContentOwnerId).OnDelete(DeleteBehavior.NoAction);
            b.HasMany(x => x.SampleVideos).WithOne().HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.PdfFiles).WithOne().HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Chapters).WithOne().HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
