using Tabsareh.Application.Contracts.Queries.Users;
using Tabsareh.Application.Contracts.QueryResult.Users;
using Tabsareh.Application.Mapper;
using Tabsareh.Domain;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class UserQueryHandler :
        IQueryHandler<GetUsersPagedQuery, GetUsersPagedQueryResult>,
        IQueryHandler<GetAllUsersQuery, List<UserItemResult>>,
        IQueryHandler<GetUserByIdQuery, UserItemResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetUsersPagedQueryResult> Handle(GetUsersPagedQuery query)
        {
            var paged = await _unitOfWork.UserRepository.GetPagedAsync(query.Options);
            return new GetUsersPagedQueryResult
            {
                Items = paged.Items.ToItemList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<List<UserItemResult>> Handle(GetAllUsersQuery query)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return users.ToItemList();
        }

        public async Task<UserItemResult> Handle(GetUserByIdQuery query)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(query.Id);
            if (user is null || user.IsDeleted) throw new NotFoundException("کاربر یافت نشد.");
            return user.ToItem();
        }
    }
}
