using Microsoft.EntityFrameworkCore;
using Tabsareh.Domain.Models.Auth;

namespace Tabsareh.Infrastructure.Persistance.Repositories
{
    public class SmsOtpRepository : ISmsOtpRepository
    {
        private readonly TabsarehDbContext _db;

        public SmsOtpRepository(TabsarehDbContext db)
        {
            _db = db;
        }

        public async Task<string> AddAsync(SmsOtp otp)
        {
            if (string.IsNullOrWhiteSpace(otp.Id))
                otp.SetId(Guid.NewGuid().ToString("N"));
            _db.SmsOtps.Add(otp);
            await _db.SaveChangesAsync();
            return otp.Id;
        }

        public async Task<SmsOtp?> GetLatestValidAsync(string phone, string code)
        {
            var now = DateTime.UtcNow;
            return await _db.SmsOtps
                .Where(x => x.Phone == phone && x.Code == code && !x.IsUsed && x.ExpiresAt >= now)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<SmsOtp> UpdateAsync(SmsOtp otp)
        {
            _db.SmsOtps.Update(otp);
            await _db.SaveChangesAsync();
            return otp;
        }
    }
}
