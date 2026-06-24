namespace Tabsareh.Application.Contracts.Contracts
{
    public interface IUserInfoService
    {
        string GetUserIdByToken();

        /// <summary>نقش کاربر از روی توکن: admin یا content_owner</summary>
        string? GetRoleByToken();
    }
}
