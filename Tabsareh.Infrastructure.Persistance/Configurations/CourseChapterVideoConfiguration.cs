using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Courses;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class CourseChapterVideoConfiguration : IEntityTypeConfiguration<CourseChapterVideo>
    {
        public void Configure(EntityTypeBuilder<CourseChapterVideo> b)
        {
            b.ToTable("CourseChapterVideos");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.CourseChapterId).HasMaxLength(64).IsRequired();
            b.Property(x => x.Title).HasMaxLength(512).IsRequired();
            b.Property(x => x.Duration).HasMaxLength(32).IsRequired();
            b.Property(x => x.ExternalVideoId).HasMaxLength(256);
            b.Property(x => x.VideoUrl).HasMaxLength(1024);
            b.Property(x => x.UploadStatus).HasMaxLength(64).IsRequired();
            b.HasIndex(x => x.CourseChapterId);
        }
    }
}
