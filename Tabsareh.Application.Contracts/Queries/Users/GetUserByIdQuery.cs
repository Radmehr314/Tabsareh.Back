using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Users
{
    public class GetUserByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
