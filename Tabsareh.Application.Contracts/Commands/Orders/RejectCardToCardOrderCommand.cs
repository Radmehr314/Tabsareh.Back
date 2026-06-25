using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Orders
{
    public class RejectCardToCardOrderCommand : ICommand
    {
        public string OrderId { get; set; }
        public string? Reason { get; set; }
    }
}
