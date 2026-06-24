using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Teacher
{
    public class GetTeacherByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
