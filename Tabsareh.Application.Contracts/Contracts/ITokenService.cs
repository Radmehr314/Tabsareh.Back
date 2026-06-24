namespace Tabsareh.Application.Contracts.Contracts
{
    public interface ITokenService
    {
        AccessToken Generate(string userId, long tokenVersion, string deviceId, string role = "user", IEnumerable<string>? permissions = null);
        string GenerateAccessToken(string userId);
    }

    public sealed record AccessToken(string Value, DateTime ExpiresAt)
    {
        public TimeSpan ExpiresIn => ExpiresAt - DateTime.UtcNow;
    }
}
