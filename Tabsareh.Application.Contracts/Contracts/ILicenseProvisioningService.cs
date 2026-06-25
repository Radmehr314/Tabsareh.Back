namespace Tabsareh.Application.Contracts.Contracts
{
    public interface ILicenseProvisioningService
    {
        Task<string> CreateLicenseAsync(string userId, string courseId, string orderId);
    }
}
