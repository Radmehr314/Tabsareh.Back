using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tabsareh.Domain.Models.Carts;
using Tabsareh.Domain.Models.Users;

namespace Tabsareh.Infrastructure.Persistance.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> b)
        {
            b.ToTable("Carts");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.UserId).HasMaxLength(64).IsRequired();
            b.HasIndex(x => x.UserId).IsUnique();
            b.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.CartId).OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> b)
        {
            b.ToTable("CartItems");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasMaxLength(64);
            b.Property(x => x.CartId).HasMaxLength(64).IsRequired();
            b.Property(x => x.CourseId).HasMaxLength(64).IsRequired();
            b.HasIndex(x => new { x.CartId, x.CourseId }).IsUnique();
        }
    }
}
