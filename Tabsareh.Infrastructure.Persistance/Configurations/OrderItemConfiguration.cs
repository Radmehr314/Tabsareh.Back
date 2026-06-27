using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Domain.Models.Orders;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> b)
        {
            b.ToTable("OrderItems");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.OrderId).HasMaxLength(64).IsRequired();
            b.Property(x => x.CourseId).HasMaxLength(64).IsRequired();
            b.Property(x => x.CourseTitleSnapshot).HasMaxLength(512).IsRequired();
            b.Property(x => x.CoursePriceSnapshot).HasColumnType("decimal(18,2)");
            b.Property(x => x.CourseDiscountPercentSnapshot).HasColumnType("decimal(5,2)");
            b.Property(x => x.CourseDiscountAmountSnapshot).HasColumnType("decimal(18,2)");
            b.Property(x => x.DiscountCodePercentSnapshot).HasColumnType("decimal(5,2)");
            b.Property(x => x.DiscountCodeAmountSnapshot).HasColumnType("decimal(18,2)");
            b.Property(x => x.LicensePriceSnapshot).HasColumnType("decimal(18,2)");
            b.Property(x => x.FinalAmount).HasColumnType("decimal(18,2)");
            b.Property(x => x.SettlementBasePriceSnapshot).HasColumnType("decimal(18,2)");
            b.Property(x => x.ContentOwnerSharePercentSnapshot).HasColumnType("decimal(5,2)");
            b.Property(x => x.ContentOwnerIdSnapshot).HasMaxLength(64).IsRequired();
            b.Property(x => x.ContentOwnerNameSnapshot).HasMaxLength(256).IsRequired();
            b.Property(x => x.LicenseCode).HasMaxLength(128);
            b.HasIndex(x => x.OrderId);
            b.HasIndex(x => x.CourseId);
            b.HasIndex(x => x.ContentOwnerIdSnapshot);
            b.HasOne<Course>().WithMany().HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
