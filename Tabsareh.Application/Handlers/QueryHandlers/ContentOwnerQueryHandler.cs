using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Contracts.Queries.ContentOwner;
using Tabsareh.Application.Contracts.QueryResult.ContentOwner;
using Tabsareh.Application.Contracts.QueryResult.Dashboard;
using Tabsareh.Application.Contracts.Queries.Dashboard;
using Tabsareh.Application.Mapper;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class ContentOwnerQueryHandler :
        IQueryHandler<GetContentOwnersPagedQuery, GetContentOwnersPagedQueryResult>,
        IQueryHandler<GetAllContentOwnersWithoutPaginationQuery, List<ContentOwnerItemResult>>,
        IQueryHandler<GetContentOwnerByIdQuery, ContentOwnerItemResult>,
        IQueryHandler<GetContentOwnerInfoByTokenQuery, GetContentOwnerInfoByTokenQueryResult>,
        IQueryHandler<GetContentOwnerPaymentsQuery, List<ContentOwnerPaymentItemResult>>,
        IQueryHandler<GetMyContentOwnerPaymentsQuery, List<ContentOwnerPaymentItemResult>>,
        IQueryHandler<GetContentOwnerDashboardStatsQuery, ContentOwnerDashboardStatsResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfoService _userInfoService;

        public ContentOwnerQueryHandler(IUnitOfWork unitOfWork, IUserInfoService userInfoService)
        {
            _unitOfWork = unitOfWork;
            _userInfoService = userInfoService;
        }

        public async Task<GetContentOwnersPagedQueryResult> Handle(GetContentOwnersPagedQuery query)
        {
            var paged = await _unitOfWork.ContentOwnerRepository.GetPagedAsync(query.Options);

            return new GetContentOwnersPagedQueryResult
            {
                Items = await MapOwners(paged.Items),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<List<ContentOwnerItemResult>> Handle(GetAllContentOwnersWithoutPaginationQuery query)
        {
            var owners = await _unitOfWork.ContentOwnerRepository.GetAllAsync();
            return await MapOwners(owners);
        }

        public async Task<ContentOwnerItemResult> Handle(GetContentOwnerByIdQuery query)
        {
            var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(query.Id);
            if (owner is null || owner.IsDeleted) throw new NotFoundException("صاحب اثر یافت نشد.");

            var earned = await _unitOfWork.OrderRepository.GetContentOwnerEarnedAmountAsync(owner.Id);
            var paid = await _unitOfWork.ContentOwnerPaymentRepository.GetPaidAmountByContentOwnerIdAsync(owner.Id);
            return owner.ToItem(Math.Max(earned - paid, 0), paid);
        }

        public async Task<GetContentOwnerInfoByTokenQueryResult> Handle(GetContentOwnerInfoByTokenQuery query)
        {
            var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(_userInfoService.GetUserIdByToken());
            if (owner is null || owner.IsDeleted) throw new NotFoundException("صاحب اثر یافت نشد.");

            return new GetContentOwnerInfoByTokenQueryResult
            {
                Id = owner.Id,
                Name = owner.Name,
                UserName = owner.UserName
            };
        }

        public async Task<List<ContentOwnerPaymentItemResult>> Handle(GetContentOwnerPaymentsQuery query)
        {
            var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(query.ContentOwnerId);
            if (owner is null || owner.IsDeleted) throw new NotFoundException("صاحب اثر یافت نشد.");
            var payments = await _unitOfWork.ContentOwnerPaymentRepository.GetByContentOwnerIdAsync(owner.Id);
            return payments.Select(x => x.ToPaymentItem(owner)).ToList();
        }

        public async Task<List<ContentOwnerPaymentItemResult>> Handle(GetMyContentOwnerPaymentsQuery query)
        {
            var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(_userInfoService.GetUserIdByToken());
            if (owner is null || owner.IsDeleted) throw new NotFoundException("صاحب اثر یافت نشد.");
            var payments = await _unitOfWork.ContentOwnerPaymentRepository.GetByContentOwnerIdAsync(owner.Id);
            return payments.Select(x => x.ToPaymentItem(owner)).ToList();
        }

        public async Task<ContentOwnerDashboardStatsResult> Handle(GetContentOwnerDashboardStatsQuery query)
        {
            var ownerId = _userInfoService.GetUserIdByToken();
            var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(ownerId);
            if (owner is null || owner.IsDeleted) throw new NotFoundException("صاحب اثر یافت نشد.");

            var earned = await _unitOfWork.OrderRepository.GetContentOwnerEarnedAmountAsync(ownerId);
            var paid = await _unitOfWork.ContentOwnerPaymentRepository.GetPaidAmountByContentOwnerIdAsync(ownerId);
            return new ContentOwnerDashboardStatsResult
            {
                TotalEarnedAmount = earned,
                PaidAmount = paid,
                PendingAmount = Math.Max(earned - paid, 0)
            };
        }

        private async Task<List<ContentOwnerItemResult>> MapOwners(IEnumerable<ContentOwner> owners)
        {
            var ownerList = owners.ToList();
            var ids = ownerList.Select(x => x.Id).ToList();
            var earned = await _unitOfWork.OrderRepository.GetContentOwnerEarnedAmountsAsync(ids);
            var paid = await _unitOfWork.ContentOwnerPaymentRepository.GetPaidAmountsByContentOwnerIdsAsync(ids);

            return ownerList.Select(owner =>
            {
                var ownerEarned = earned.TryGetValue(owner.Id, out var earnedAmount) ? earnedAmount : 0m;
                var ownerPaid = paid.TryGetValue(owner.Id, out var paidAmount) ? paidAmount : 0m;
                return owner.ToItem(Math.Max(ownerEarned - ownerPaid, 0), ownerPaid);
            }).ToList();
        }
    }
}
