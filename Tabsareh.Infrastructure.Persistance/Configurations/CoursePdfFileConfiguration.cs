using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Courses;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class CoursePdfFileConfiguration : IEntityTypeConfiguration<CoursePdfFile>
    {
        public void Configure(EntityTypeBuilder<CoursePdfFile> b)
        {
            b.ToTable("CoursePdfFiles");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.CourseId).HasMaxLength(64).IsRequired();
            b.Property(x => x.Title).HasMaxLength(512).IsRequired();
            b.Property(x => x.FileUrl).HasMaxLength(1024).IsRequired();
            b.HasIndex(x => x.CourseId);
        }
    }
}
