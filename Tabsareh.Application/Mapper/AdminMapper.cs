using Tabsareh.Framework.Application.Convertor;
using Tabsareh.Application.Contracts.QueryResult.Admin;
using Tabsareh.Domain.Common;
using Tabsareh.Domain.Models.Admins;
using Tabsareh.Domain.Models.Roles;

namespace Tabsareh.Application.Mapper;

public static class AdminMapper
{
    public static GetAllAdminItemResult ToQueryItem(this Admin admin, Role? role = null)
    {
        if (admin == null)
            throw new ArgumentNullException(nameof(admin));

        return new GetAllAdminItemResult
        {
            Id = admin.Id,
            Username = admin.UserName,
            FirstName = admin.FirstName,
            LastName = admin.LastName,
            Phone = admin.Phone,
            IsBan = admin.IsBan,
            RoleId = admin.RoleId,
            RoleName = role?.Name,
            CreatedAt = admin.CreatedAt.ToShamsiDate()
        };
    }

    public static List<GetAllAdminItemResult> ToQueryItemList(this IEnumerable<Admin> admins)
    {
        return admins.Select(t => t.ToQueryItem()).ToList();
    }

    public static GetAllAdminQueryResult ToQueryResult(this PagedResult<Admin> paged)
    {
        if (paged == null)
            throw new ArgumentNullException(nameof(paged));

        return new GetAllAdminQueryResult
        {
            Items = paged.Items.ToQueryItemList(),
            TotalCount = (int)paged.TotalCount,
            Skip = paged.Skip,
            Limit = paged.Limit
        };
    }
}
