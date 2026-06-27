using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Cart
{
    public class RemoveFromCartCommand : ICommand
    {
        public string CourseId { get; set; }
    }
}
