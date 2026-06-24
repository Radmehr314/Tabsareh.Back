using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Role
{
    public class GetRoleByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
