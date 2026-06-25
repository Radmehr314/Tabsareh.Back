using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Discounts
{
    public class AddDiscountCodeCommand : ICommand
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public int UsageLimit { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
