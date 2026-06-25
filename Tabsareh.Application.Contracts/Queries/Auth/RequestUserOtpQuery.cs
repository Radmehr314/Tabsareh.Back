using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Auth
{
    public class RequestUserOtpQuery : IQuery
    {
        public string Phone { get; set; }
    }
}
