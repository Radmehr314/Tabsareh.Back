namespace Tabsareh.Application.Contracts.Contracts
{
    public interface ISmsService
    {
        Task<bool> SendOtpAsync(string mobile, string code);
    }
}
