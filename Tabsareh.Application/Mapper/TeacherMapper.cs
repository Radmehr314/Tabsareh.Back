using Tabsareh.Framework.Application.Convertor;
using Tabsareh.Application.Contracts.QueryResult.Teacher;
using Tabsareh.Domain.Models.Teachers;

namespace Tabsareh.Application.Mapper;

public static class TeacherMapper
{
    public static TeacherItemResult ToItem(this Teacher teacher)
    {
        if (teacher == null)
            throw new ArgumentNullException(nameof(teacher));

        return new TeacherItemResult
        {
            Id = teacher.Id,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            ProfileImage = teacher.ProfileImage,
            Description = teacher.Description,
            CreatedAt = teacher.CreatedAt.ToShamsiDate()
        };
    }

    public static List<TeacherItemResult> ToItemList(this IEnumerable<Teacher> teachers)
    {
        return teachers.Select(t => t.ToItem()).ToList();
    }
}
