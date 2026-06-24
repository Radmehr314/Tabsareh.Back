using Tabsareh.Framework.Application.Convertor;
using Tabsareh.Application.Contracts.QueryResult.ContentOwner;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Domain.Models.Teachers;

namespace Tabsareh.Application.Mapper;

public static class ContentOwnerMapper
{
    public static ContentOwnerItemResult ToItem(this ContentOwner owner, IReadOnlyDictionary<string, Teacher>? teachers = null)
    {
        if (owner == null)
            throw new ArgumentNullException(nameof(owner));

        return new ContentOwnerItemResult
        {
            Id = owner.Id,
            Name = owner.Name,
            UserName = owner.UserName,
            IsBan = owner.IsBan,
            TeacherIds = owner.TeacherIds,
            Teachers = owner.TeacherIds.ToBriefList(teachers),
            CreatedAt = owner.CreatedAt.ToShamsiDate()
        };
    }

    public static List<TeacherBriefDto> ToBriefList(this IEnumerable<string> teacherIds, IReadOnlyDictionary<string, Teacher>? teachers)
    {
        if (teachers == null)
            return new List<TeacherBriefDto>();

        return teacherIds
            .Where(teachers.ContainsKey)
            .Select(id => new TeacherBriefDto
            {
                Id = id,
                FullName = $"{teachers[id].FirstName} {teachers[id].LastName}".Trim()
            })
            .ToList();
    }
}
