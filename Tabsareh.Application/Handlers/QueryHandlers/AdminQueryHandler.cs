using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;
using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Contracts.Queries.Admin;
using Tabsareh.Application.Contracts.QueryResult.Admin;
using Tabsareh.Application.Mapper;
using Tabsareh.Domain;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class AdminQueryHandler :
        IQueryHandler<GetAllAdminQuery, GetAllAdminQueryResult>,
        IQueryHandler<GetAllAdminsWithoutPaginationQuery, List<GetAllAdminItemResult>>,
        IQueryHandler<GetAdminByIdQuery, GetAllAdminItemResult>,
        IQueryHandler<GetAdminInfoByTokenQuery, GetAdminInfoByTokenQueryResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfoService _userInfoService;

        public AdminQueryHandler(IUnitOfWork unitOfWork, IUserInfoService userInfoService)
        {
            _unitOfWork = unitOfWork;
            _userInfoService = userInfoService;
        }

        public async Task<GetAllAdminQueryResult> Handle(GetAllAdminQuery query)
        {
            var paged = await _unitOfWork.AdminRepository.GetPagedAsync(query.Options);

            var roles = (await _unitOfWork.RoleRepository.GetAllAsync())
                .ToDictionary(r => r.Id);

            var items = paged.Items.Select(admin =>
            {
                roles.TryGetValue(admin.RoleId ?? "", out var role);
                return admin.ToQueryItem(role);
            }).ToList();

            return new GetAllAdminQueryResult
            {
                Items = items,
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<List<GetAllAdminItemResult>> Handle(GetAllAdminsWithoutPaginationQuery query)
        {
            var admins = await _unitOfWork.AdminRepository.GetAllAsync();

            var roles = (await _unitOfWork.RoleRepository.GetAllAsync())
                .ToDictionary(r => r.Id);

            return admins.Select(admin =>
            {
                roles.TryGetValue(admin.RoleId ?? "", out var role);
                return admin.ToQueryItem(role);
            }).ToList();
        }

        public async Task<GetAllAdminItemResult> Handle(GetAdminByIdQuery query)
        {
            var admin = await _unitOfWork.AdminRepository.GetByIdAsync(query.Id);
            if (admin is null) throw new NotFoundException("ادمین یافت نشد.");

            var role = !string.IsNullOrWhiteSpace(admin.RoleId)
                ? await _unitOfWork.RoleRepository.GetByIdAsync(admin.RoleId)
                : null;

            return admin.ToQueryItem(role);
        }

        public async Task<GetAdminInfoByTokenQueryResult> Handle(GetAdminInfoByTokenQuery query)
        {
            var user = await _unitOfWork.AdminRepository.GetByIdAsync(_userInfoService.GetUserIdByToken());
            if (user is null) throw new NotFoundException("ادمین یافت نشد.");

            var role = !string.IsNullOrWhiteSpace(user.RoleId)
                ? await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId)
                : null;

            return new GetAdminInfoByTokenQueryResult
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = role?.Name,
                Permissions = role?.Permissions ?? new List<string>()
            };
        }
    }
}
