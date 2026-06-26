using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.CourseComments;
using Tabsareh.Domain.Models.Courses;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class CourseCommentConfiguration : IEntityTypeConfiguration<CourseComment>
    {
        public void Configure(EntityTypeBuilder<CourseComment> b)
        {
            b.ToTable("CourseComments");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.CourseId).HasMaxLength(64).IsRequired();
            b.Property(x => x.AuthorName).HasMaxLength(256).IsRequired();
            b.Property(x => x.AuthorPhone).HasMaxLength(20);
            b.Property(x => x.Content).HasMaxLength(2000).IsRequired();
            b.Property(x => x.Rating).IsRequired();
            b.HasIndex(x => x.CourseId);
            b.HasIndex(x => x.IsApproved);
            b.HasIndex(x => x.IsDeleted);

            b.HasOne<Course>().WithMany().HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
