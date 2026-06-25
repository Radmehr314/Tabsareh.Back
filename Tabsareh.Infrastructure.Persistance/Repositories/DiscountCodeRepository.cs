using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Discounts;
using Tabsareh.Infrastructure.Persistance.Common;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class DiscountCodeRepository : IDiscountCodeRepository
    {
        private readonly TabsarehDbContext _db;

        public DiscountCodeRepository(TabsarehDbContext db) => _db = db;

        public async Task<DiscountCode?> GetByIdAsync(string id)
            => await _db.DiscountCodes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> ExistsByCodeAsync(string code, string? ignoreId = null)
            => await _db.DiscountCodes.AnyAsync(x => x.Code == code.Trim().ToUpper() && !x.IsDeleted && x.Id != ignoreId);

        public async Task<PagedResult<DiscountCode>> GetPagedAsync(QueryOptions options)
        {
            var query = _db.DiscountCodes.AsNoTracking().Where(x => !x.IsDeleted);
            return await QueryHelper.GetPagedResultAsync(query, options);
        }

        public async Task<string> AddAsync(DiscountCode discountCode)
        {
            if (string.IsNullOrWhiteSpace(discountCode.Id))
                discountCode.SetId(Guid.NewGuid().ToString("N"));
            _db.DiscountCodes.Add(discountCode);
            await _db.SaveChangesAsync();
            return discountCode.Id;
        }

        public async Task<DiscountCode> UpdateAsync(DiscountCode discountCode)
        {
            var exists = await _db.DiscountCodes.AnyAsync(x => x.Id == discountCode.Id);
            if (!exists) throw new InvalidOperationException("Discount code not found.");
            _db.DiscountCodes.Update(discountCode);
            await _db.SaveChangesAsync();
            return discountCode;
        }
    }
}
