namespace Tabsareh.Domain.Models.ContentOwners
{
    public interface IContentOwnerPaymentRepository
    {
        Task<string> AddAsync(ContentOwnerPayment payment);
        Task<List<ContentOwnerPayment>> GetByContentOwnerIdAsync(string contentOwnerId);
        Task<decimal> GetPaidAmountByContentOwnerIdAsync(string contentOwnerId);
        Task<Dictionary<string, decimal>> GetPaidAmountsByContentOwnerIdsAsync(IEnumerable<string> contentOwnerIds);
    }
}
