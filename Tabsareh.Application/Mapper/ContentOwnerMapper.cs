using Tabsareh.Application.Contracts.QueryResult.ContentOwner;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Framework.Application.Convertor;

namespace Tabsareh.Application.Mapper;

public static class ContentOwnerMapper
{
    public static ContentOwnerItemResult ToItem(this ContentOwner owner)
    {
        if (owner == null)
            throw new ArgumentNullException(nameof(owner));

        return new ContentOwnerItemResult
        {
            Id = owner.Id,
            Name = owner.Name,
            UserName = owner.UserName,
            IsBan = owner.IsBan,
            CreatedAt = owner.CreatedAt.ToShamsiDate()
        };
    }
}
