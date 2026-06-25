using Tabsareh.Application.Contracts.Contracts;

namespace Tabsareh.Infrastructure.Persistance.Services
{
    public class LicenseProvisioningService : ILicenseProvisioningService
    {
        public Task<string> CreateLicenseAsync(string userId, string courseId, string orderId)
        {
            // Placeholder until the external license API is available.
            return Task.FromResult($"LIC-{DateTime.Now:yyyyMMdd}-{orderId[..Math.Min(8, orderId.Length)].ToUpper()}");
        }
    }
}
