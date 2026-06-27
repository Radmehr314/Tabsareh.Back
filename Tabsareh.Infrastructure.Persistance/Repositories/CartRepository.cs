using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Models.Carts;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly TabsarehDbContext _db;

        public CartRepository(TabsarehDbContext db) => _db = db;

        public async Task<Cart?> GetByUserIdAsync(string userId)
            => await _db.Carts
                .Include(x => x.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<Cart> GetOrCreateAsync(string userId)
        {
            var exists = await _db.Carts.AnyAsync(x => x.UserId == userId);
            if (!exists)
            {
                var newCart = new Cart(userId);
                newCart.SetId(Guid.NewGuid().ToString("N"));
                _db.Carts.Add(newCart);
                await _db.SaveChangesAsync();
                _db.Entry(newCart).State = EntityState.Detached;
            }

            return await GetByUserIdAsync(userId)
                ?? throw new InvalidOperationException("Cart could not be created.");
        }

        public async Task UpdateAsync(Cart cart)
        {
            // حذف آیتم‌های قدیمی مستقیم از DB
            await _db.CartItems.Where(x => x.CartId == cart.Id).ExecuteDeleteAsync();

            // درج آیتم‌های جدید به صورت fresh (بدون tracking conflict)
            foreach (var item in cart.Items)
            {
                var freshItem = new CartItem(cart.Id, item.CourseId);
                freshItem.SetId(Guid.NewGuid().ToString("N"));
                _db.CartItems.Add(freshItem);
            }

            await _db.SaveChangesAsync();
        }
    }
}
