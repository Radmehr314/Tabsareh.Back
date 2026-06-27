using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Cart
{
    public class CreateOrderFromCartCommand : ICommand
    {
        public string PaymentMethod { get; set; }
        public string? DiscountCode { get; set; }
        public string? CardToCardTrackingNumber { get; set; }
        public string? CardToCardDescription { get; set; }
    }
}
