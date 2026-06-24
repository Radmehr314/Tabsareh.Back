using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Categories
{
    public class DeleteCategoryCommand : ICommand
    {
        public string Id { get; set; }
    }
}
