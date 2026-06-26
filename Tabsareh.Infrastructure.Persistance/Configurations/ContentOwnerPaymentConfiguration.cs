using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.ContentOwners;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class ContentOwnerPaymentConfiguration : IEntityTypeConfiguration<ContentOwnerPayment>
    {
        public void Configure(EntityTypeBuilder<ContentOwnerPayment> b)
        {
            b.ToTable("ContentOwnerPayments");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.ContentOwnerId).HasMaxLength(64).IsRequired();
            b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            b.Property(x => x.ReceiptImage).HasMaxLength(1024).IsRequired();
            b.Property(x => x.TrackingNumber).HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(1024);
            b.HasIndex(x => x.ContentOwnerId);
            b.HasOne<ContentOwner>()
                .WithMany()
                .HasForeignKey(x => x.ContentOwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
