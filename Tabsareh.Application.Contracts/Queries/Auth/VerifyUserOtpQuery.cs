using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Auth
{
    public class VerifyUserOtpQuery : IQuery
    {
        public string Phone { get; set; }
        public string Code { get; set; }
    }
}
