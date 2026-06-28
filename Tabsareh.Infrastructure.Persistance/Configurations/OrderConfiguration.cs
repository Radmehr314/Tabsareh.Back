using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Courses;
using Tabsareh.Domain.Models.Orders;
using Tabsareh.Domain.Models.Users;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> b)
        {
            b.ToTable("Orders");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.UserId).HasMaxLength(64).IsRequired();
            b.Property(x => x.CourseId).HasMaxLength(64);
            b.Property(x => x.PaymentMethod).HasMaxLength(32).IsRequired();
            b.Property(x => x.Status).HasMaxLength(32).IsRequired();
            b.Property(x => x.CoursePrice).HasColumnType("decimal(18,2)");
            b.Property(x => x.DiscountPercentSnapshot).HasColumnType("decimal(5,2)");
            b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            b.Property(x => x.SettlementBasePriceSnapshot).HasColumnType("decimal(18,2)");
            b.Property(x => x.ContentOwnerSharePercentSnapshot).HasColumnType("decimal(5,2)");
            b.Property(x => x.SubtotalAmount).HasColumnType("decimal(18,2)");
            b.Property(x => x.CourseDiscountAmount).HasColumnType("decimal(18,2)");
            b.Property(x => x.DiscountCodeAmount).HasColumnType("decimal(18,2)");
            b.Property(x => x.PayableAmount).HasColumnType("decimal(18,2)");
            b.Property(x => x.DiscountCode).HasMaxLength(64);
            b.Property(x => x.DiscountCodePercentSnapshot).HasColumnType("decimal(5,2)");
            b.Property(x => x.CardToCardTrackingNumber).HasMaxLength(128);
            b.Property(x => x.CardToCardDescription).HasMaxLength(1024);
            b.Property(x => x.RejectionReason).HasMaxLength(1024);
            b.Property(x => x.LicenseCode).HasMaxLength(128);
            b.Property(x => x.GatewayToken).HasMaxLength(256);
            b.Property(x => x.GatewayRefNum).HasMaxLength(128);
            b.Property(x => x.GatewayRRN).HasMaxLength(64);
            b.Property(x => x.GatewayTraceNo).HasMaxLength(64);
            b.Property(x => x.GatewaySecurePan).HasMaxLength(32);
            b.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
            b.Navigation(x => x.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
            b.HasIndex(x => x.UserId);
            b.HasIndex(x => x.CourseId);
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.PaymentMethod);
            b.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            b.HasOne<Course>().WithMany().HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
        }
    }
}
