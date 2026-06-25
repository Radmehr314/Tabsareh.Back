using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Discounts
{
    public class DeleteDiscountCodeCommand : ICommand
    {
        public string Id { get; set; }
    }
}
