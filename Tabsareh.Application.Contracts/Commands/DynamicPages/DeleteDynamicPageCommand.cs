using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.DynamicPages
{
    public class DeleteDynamicPageCommand : ICommand
    {
        public string Id { get; set; }
    }
}
