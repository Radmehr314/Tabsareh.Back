using Tabsareh.Domain.Common;

namespace Tabsareh.Domain.Models.Discounts
{
    public interface IDiscountCodeRepository
    {
        Task<DiscountCode?> GetByIdAsync(string id);
        Task<bool> ExistsByCodeAsync(string code, string? ignoreId = null);
        Task<PagedResult<DiscountCode>> GetPagedAsync(QueryOptions options);
        Task<string> AddAsync(DiscountCode discountCode);
        Task<DiscountCode> UpdateAsync(DiscountCode discountCode);
    }
}
