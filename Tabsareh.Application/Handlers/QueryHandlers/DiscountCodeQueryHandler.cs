using Tabsareh.Application.Contracts.Queries.Discounts;
using Tabsareh.Application.Contracts.QueryResult.Discounts;
using Tabsareh.Application.Mapper;
using Tabsareh.Domain;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class DiscountCodeQueryHandler :
        IQueryHandler<GetDiscountCodesPagedQuery, GetDiscountCodesPagedQueryResult>,
        IQueryHandler<GetDiscountCodeByIdQuery, DiscountCodeItemResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountCodeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetDiscountCodesPagedQueryResult> Handle(GetDiscountCodesPagedQuery query)
        {
            var paged = await _unitOfWork.DiscountCodeRepository.GetPagedAsync(query.Options);
            return new GetDiscountCodesPagedQueryResult
            {
                Items = paged.Items.Select(x => x.ToItem()).ToList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<DiscountCodeItemResult> Handle(GetDiscountCodeByIdQuery query)
        {
            var discountCode = await _unitOfWork.DiscountCodeRepository.GetByIdAsync(query.Id);
            if (discountCode is null || discountCode.IsDeleted) throw new NotFoundException("کد تخفیف یافت نشد.");
            return discountCode.ToItem();
        }
    }
}
