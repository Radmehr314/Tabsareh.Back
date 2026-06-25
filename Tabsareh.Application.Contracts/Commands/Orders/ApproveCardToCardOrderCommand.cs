using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Orders
{
    public class ApproveCardToCardOrderCommand : ICommand
    {
        public string OrderId { get; set; }
    }
}
