namespace Tabsareh.Application.Contracts.QueryResult.Auth
{
    public class LoginResultDto
    {
        public string AccessToken { get; init; } = default!;
        public string TokenType { get; init; } = "Bearer";
        public int ExpiresIn { get; init; }

        /// <summary>نوع کاربر واردشده: admin یا content_owner</summary>
        public string Role { get; init; } = default!;
    }
}
