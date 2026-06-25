using Tabsareh.Application.Contracts.QueryResult.Discounts;
using Tabsareh.Domain.Models.Discounts;
using Tabsareh.Framework.Application.Convertor;

namespace Tabsareh.Application.Mapper
{
    public static class DiscountCodeMapper
    {
        public static DiscountCodeItemResult ToItem(this DiscountCode discountCode)
        {
            return new DiscountCodeItemResult
            {
                Id = discountCode.Id,
                Title = discountCode.Title,
                Code = discountCode.Code,
                UsageLimit = discountCode.UsageLimit,
                UsedCount = discountCode.UsedCount,
                DiscountPercent = discountCode.DiscountPercent,
                StartDate = discountCode.StartDate,
                EndDate = discountCode.EndDate,
                StartDatePersian = discountCode.StartDate.ToShamsiDate(),
                EndDatePersian = discountCode.EndDate.ToShamsiDate(),
                CreatedAt = discountCode.CreatedAt.ToShamsiDate()
            };
        }
    }
}
