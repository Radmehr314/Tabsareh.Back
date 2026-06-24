using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;
using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Contracts.Queries.ContentOwner;
using Tabsareh.Application.Contracts.QueryResult.ContentOwner;
using Tabsareh.Application.Mapper;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.Teachers;

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
            var teachers = await BuildTeacherMap(paged.Items.SelectMany(o => o.TeacherIds));

            return new GetContentOwnersPagedQueryResult
            {
                Items = paged.Items.Select(o => o.ToItem(teachers)).ToList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<List<ContentOwnerItemResult>> Handle(GetAllContentOwnersWithoutPaginationQuery query)
        {
            var owners = (await _unitOfWork.ContentOwnerRepository.GetAllAsync()).ToList();
            var teachers = await BuildTeacherMap(owners.SelectMany(o => o.TeacherIds));
            return owners.Select(o => o.ToItem(teachers)).ToList();
        }

        public async Task<ContentOwnerItemResult> Handle(GetContentOwnerByIdQuery query)
        {
            var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(query.Id);
            if (owner is null || owner.IsDeleted) throw new NotFoundException("صاحب اثر یافت نشد.");
            var teachers = await BuildTeacherMap(owner.TeacherIds);
            return owner.ToItem(teachers);
        }

        public async Task<GetContentOwnerInfoByTokenQueryResult> Handle(GetContentOwnerInfoByTokenQuery query)
        {
            var owner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(_userInfoService.GetUserIdByToken());
            if (owner is null || owner.IsDeleted) throw new NotFoundException("صاحب اثر یافت نشد.");
            var teachers = await BuildTeacherMap(owner.TeacherIds);

            return new GetContentOwnerInfoByTokenQueryResult
            {
                Id = owner.Id,
                Name = owner.Name,
                UserName = owner.UserName,
                TeacherIds = owner.TeacherIds,
                Teachers = owner.TeacherIds.ToBriefList(teachers)
            };
        }

        private async Task<Dictionary<string, Teacher>> BuildTeacherMap(IEnumerable<string> teacherIds)
        {
            var ids = teacherIds.Distinct().ToList();
            if (ids.Count == 0) return new Dictionary<string, Teacher>();
            var teachers = await _unitOfWork.TeacherRepository.GetByIdsAsync(ids);
            return teachers.ToDictionary(t => t.Id);
        }
    }
}
