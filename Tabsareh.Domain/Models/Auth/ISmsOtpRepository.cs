namespace Tabsareh.Domain.Models.Auth
{
    public interface ISmsOtpRepository
    {
        Task<string> AddAsync(SmsOtp otp);
        Task<SmsOtp?> GetLatestValidAsync(string phone, string code);
        Task<SmsOtp> UpdateAsync(SmsOtp otp);
    }
}
