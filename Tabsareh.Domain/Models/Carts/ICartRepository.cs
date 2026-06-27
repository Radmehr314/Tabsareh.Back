namespace Tabsareh.Domain.Models.Carts
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(string userId);
        Task<Cart> GetOrCreateAsync(string userId);
        Task UpdateAsync(Cart cart);
    }
}
