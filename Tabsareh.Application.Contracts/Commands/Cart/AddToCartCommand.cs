using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Cart
{
    public class AddToCartCommand : ICommand
    {
        public string CourseId { get; set; }
    }
}
