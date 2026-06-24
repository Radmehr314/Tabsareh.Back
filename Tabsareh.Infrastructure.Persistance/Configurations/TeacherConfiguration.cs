using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Teachers;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> b)
        {
            b.ToTable("Teachers");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.FirstName).HasMaxLength(256);
            b.Property(x => x.LastName).HasMaxLength(256);
        }
    }
}
