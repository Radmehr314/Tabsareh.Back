using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Orders;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TabsarehDbContext _db;

        public OrderRepository(TabsarehDbContext db) => _db = db;

        public async Task<Order?> GetByIdAsync(string id)
            => await _db.Orders.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);

        public async Task<PagedResult<OrderListItem>> GetPagedAsync(QueryOptions options, OrderFilter filter)
        {
            var query = BuildListQuery(filter);
            var totalCount = await query.LongCountAsync();
            var items = await query
                .OrderByDescending(x => x.Order.CreatedAt)
                .Skip(options.Skip)
                .Take(options.Limit)
                .ToListAsync();

            return new PagedResult<OrderListItem>
            {
                Items = items,
                TotalCount = totalCount,
                Skip = options.Skip,
                Limit = options.Limit
            };
        }

        public async Task<List<OrderListItem>> GetByUserIdAsync(string userId)
            => await BuildListQuery(new OrderFilter())
                .Where(x => x.Order.UserId == userId)
                .OrderByDescending(x => x.Order.CreatedAt)
                .ToListAsync();

        public async Task<string> AddAsync(Order order)
        {
            if (string.IsNullOrWhiteSpace(order.Id))
                order.SetId(Guid.NewGuid().ToString("N"));
            foreach (var item in order.Items)
                if (string.IsNullOrWhiteSpace(item.Id)) item.SetId(Guid.NewGuid().ToString("N"));
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order.Id;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            var exists = await _db.Orders.AnyAsync(x => x.Id == order.Id);
            if (!exists) throw new InvalidOperationException("Order not found.");
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return order;
        }

        private IQueryable<OrderListItem> BuildListQuery(OrderFilter filter)
        {
            var query =
                from order in _db.Orders.AsNoTracking().Include(x => x.Items)
                join user in _db.Users.AsNoTracking() on order.UserId equals user.Id
                select new OrderListItem
                {
                    Order = order,
                    UserPhone = user.Phone,
                    UserFullName = (user.FirstName + " " + user.LastName).Trim(),
                    CourseTitle = order.Items.Select(x => x.CourseTitleSnapshot).FirstOrDefault() ?? string.Empty
                };

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim();
                query = query.Where(x =>
                    x.UserPhone.Contains(search) ||
                    x.Order.Items.Any(i => i.CourseTitleSnapshot.Contains(search)));
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
                query = query.Where(x => x.Order.Status == filter.Status);

            if (!string.IsNullOrWhiteSpace(filter.PaymentMethod))
                query = query.Where(x => x.Order.PaymentMethod == filter.PaymentMethod);

            if (!string.IsNullOrWhiteSpace(filter.ContentOwnerId))
                query = query.Where(x => x.Order.Items.Any(i => i.ContentOwnerIdSnapshot == filter.ContentOwnerId));

            if (filter.FromDate.HasValue)
                query = query.Where(x => x.Order.CreatedAt.Date >= filter.FromDate.Value.Date);

            if (filter.ToDate.HasValue)
                query = query.Where(x => x.Order.CreatedAt.Date <= filter.ToDate.Value.Date);

            return query;
        }
    }
}
