using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Courses;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class CourseChapterConfiguration : IEntityTypeConfiguration<CourseChapter>
    {
        public void Configure(EntityTypeBuilder<CourseChapter> b)
        {
            b.ToTable("CourseChapters");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.CourseId).HasMaxLength(64).IsRequired();
            b.Property(x => x.Title).HasMaxLength(512).IsRequired();
            b.Property(x => x.Duration).HasMaxLength(32).IsRequired();
            b.HasIndex(x => x.CourseId);
            b.HasIndex(x => x.DisplayOrder);
            b.HasMany(x => x.Videos).WithOne().HasForeignKey(x => x.CourseChapterId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
