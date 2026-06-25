using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Auth;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class SmsOtpConfiguration : IEntityTypeConfiguration<SmsOtp>
    {
        public void Configure(EntityTypeBuilder<SmsOtp> b)
        {
            b.ToTable("SmsOtps");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.Phone).HasMaxLength(32).IsRequired();
            b.Property(x => x.Code).HasMaxLength(16).IsRequired();
            b.HasIndex(x => x.Phone);
            b.HasIndex(x => new { x.Phone, x.Code, x.IsUsed, x.ExpiresAt });
        }
    }
}
