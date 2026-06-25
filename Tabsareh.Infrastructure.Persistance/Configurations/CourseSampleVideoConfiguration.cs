using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Courses;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class CourseSampleVideoConfiguration : IEntityTypeConfiguration<CourseSampleVideo>
    {
        public void Configure(EntityTypeBuilder<CourseSampleVideo> b)
        {
            b.ToTable("CourseSampleVideos");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.CourseId).HasMaxLength(64).IsRequired();
            b.Property(x => x.Title).HasMaxLength(512).IsRequired();
            b.Property(x => x.FileUrl).HasMaxLength(1024).IsRequired();
            b.Property(x => x.Duration).HasMaxLength(16).IsRequired();
            b.HasIndex(x => x.CourseId);
        }
    }
}
