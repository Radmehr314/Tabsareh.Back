using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Models.ContentOwners;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class ContentOwnerPaymentRepository : IContentOwnerPaymentRepository
    {
        private readonly TabsarehDbContext _db;

        public ContentOwnerPaymentRepository(TabsarehDbContext db) => _db = db;

        public async Task<string> AddAsync(ContentOwnerPayment payment)
        {
            if (string.IsNullOrWhiteSpace(payment.Id))
                payment.SetId(Guid.NewGuid().ToString("N"));
            _db.ContentOwnerPayments.Add(payment);
            await _db.SaveChangesAsync();
            return payment.Id;
        }

        public async Task<List<ContentOwnerPayment>> GetByContentOwnerIdAsync(string contentOwnerId)
            => await _db.ContentOwnerPayments.AsNoTracking()
                .Where(x => x.ContentOwnerId == contentOwnerId)
                .OrderByDescending(x => x.PaymentDate)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync();

        public async Task<decimal> GetPaidAmountByContentOwnerIdAsync(string contentOwnerId)
            => await _db.ContentOwnerPayments.AsNoTracking()
                .Where(x => x.ContentOwnerId == contentOwnerId)
                .SumAsync(x => (decimal?)x.Amount) ?? 0m;

        public async Task<Dictionary<string, decimal>> GetPaidAmountsByContentOwnerIdsAsync(IEnumerable<string> contentOwnerIds)
        {
            var ids = contentOwnerIds.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
            return await _db.ContentOwnerPayments.AsNoTracking()
                .Where(x => ids.Contains(x.ContentOwnerId))
                .GroupBy(x => x.ContentOwnerId)
                .Select(x => new { ContentOwnerId = x.Key, Amount = x.Sum(p => p.Amount) })
                .ToDictionaryAsync(x => x.ContentOwnerId, x => x.Amount);
        }
    }
}
