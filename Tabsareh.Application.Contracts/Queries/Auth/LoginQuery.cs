using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Auth
{
    public class LoginQuery : IQuery
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
