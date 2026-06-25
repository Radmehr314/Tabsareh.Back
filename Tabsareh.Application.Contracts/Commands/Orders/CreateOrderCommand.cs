using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Orders
{
    public class CreateOrderCommand : ICommand
    {
        public List<string> CourseIds { get; set; } = new();
        public string PaymentMethod { get; set; }
        public string? DiscountCode { get; set; }
        public string? CardToCardTrackingNumber { get; set; }
        public string? CardToCardDescription { get; set; }
    }
}
