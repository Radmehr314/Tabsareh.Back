using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Discounts;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode>
    {
        public void Configure(EntityTypeBuilder<DiscountCode> b)
        {
            b.ToTable("DiscountCodes");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.Title).HasMaxLength(256).IsRequired();
            b.Property(x => x.Code).HasMaxLength(64).IsRequired();
            b.Property(x => x.DiscountPercent).HasColumnType("decimal(5,2)");
            b.HasIndex(x => x.Code).IsUnique();
            b.HasIndex(x => x.IsDeleted);
        }
    }
}
