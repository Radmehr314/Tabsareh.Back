using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.ContentOwners
{
    public class AddContentOwnerPaymentCommand : ICommand
    {
        public string ContentOwnerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReceiptImage { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Description { get; set; }
    }
}
