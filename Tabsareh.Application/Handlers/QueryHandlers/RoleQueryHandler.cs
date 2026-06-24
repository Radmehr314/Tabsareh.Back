using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Convertor;
using Tabsareh.Framework.Application.Exceptions;
using Tabsareh.Application.Contracts.Queries.Role;
using Tabsareh.Application.Contracts.QueryResult.Role;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Permissions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class RoleQueryHandler :
        IQueryHandler<GetAllRolesQuery, List<RoleDto>>,
        IQueryHandler<GetRolesPagedQuery, GetRolesPagedQueryResult>,
        IQueryHandler<GetRoleByIdQuery, RoleDto>,
        IQueryHandler<GetAllPermissionsQuery, List<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoleDto>> Handle(GetAllRolesQuery query)
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync();
            return roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Permissions = r.Permissions,
                CreatedAt = r.CreatedAt.ToShamsiDate()
            }).ToList();
        }

        public async Task<GetRolesPagedQueryResult> Handle(GetRolesPagedQuery query)
        {
            var paged = await _unitOfWork.RoleRepository.GetPagedAsync(query.Options);
            return new GetRolesPagedQueryResult
            {
                Items = paged.Items.Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Permissions = r.Permissions,
                    CreatedAt = r.CreatedAt.ToShamsiDate()
                }).ToList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<RoleDto> Handle(GetRoleByIdQuery query)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(query.Id);
            if (role is null) throw new NotFoundException("نقش مورد نظر یافت نشد.");

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = role.Permissions,
                CreatedAt = role.CreatedAt.ToShamsiDate()
            };
        }

        public Task<List<string>> Handle(GetAllPermissionsQuery query)
        {
            return Task.FromResult(Permission.All.ToList());
        }
    }
}
