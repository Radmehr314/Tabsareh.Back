namespace Tabsareh.Application.Contracts.QueryResult.Auth
{
    public class RequestUserOtpResultDto
    {
        public string Id { get; set; }
        public string Phone { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}
