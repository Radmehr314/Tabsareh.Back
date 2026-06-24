using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Contracts.Queries.ContentOwner;
using Tabsareh.Application.Contracts.QueryResult.ContentOwner;
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
        IQueryHandler<GetContentOwnerInfoByTokenQuery, GetContentOwnerInfoByTokenQueryResult>
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
                Items = paged.Items.Select(o => o.ToItem()).ToList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<List<ContentOwnerItemResult>> Handle(GetAllContentOwnersWithoutPaginationQuery query)
        {
            var owners = await _unitOfWork.ContentOwnerRepository.GetAllAsync();
            return owners.Select(o => o.ToItem()).ToList();
        }

        public async Task<ContentOwnerItemResult> Handle(GetContentOwnerByIdQuery query)
        {
            var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(query.Id);
            if (owner is null || owner.IsDeleted) throw new NotFoundException("صاحب اثر یافت نشد.");
            return owner.ToItem();
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
    }
}
