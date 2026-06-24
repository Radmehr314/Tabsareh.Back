using Tabsareh.Application.Contracts.QueryResult.Users;
using Tabsareh.Domain.Models.Users;
using Tabsareh.Framework.Application.Convertor;

namespace Tabsareh.Application.Mapper;

public static class UserMapper
{
    public static UserItemResult ToItem(this User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        return new UserItemResult
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Phone = user.Phone,
            CreatedAt = user.CreatedAt.ToShamsiDate()
        };
    }

    public static List<UserItemResult> ToItemList(this IEnumerable<User> users)
    {
        return users.Select(u => u.ToItem()).ToList();
    }
}
