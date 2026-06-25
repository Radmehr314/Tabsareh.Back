using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.Orders
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(string id);
        Task<PagedResult<OrderListItem>> GetPagedAsync(QueryOptions options, OrderFilter filter);
        Task<List<OrderListItem>> GetByUserIdAsync(string userId);
        Task<string> AddAsync(Order order);
        Task<Order> UpdateAsync(Order order);
    }
}
