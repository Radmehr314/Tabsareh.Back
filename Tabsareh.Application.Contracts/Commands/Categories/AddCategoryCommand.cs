using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Categories
{
    public class AddCategoryCommand : ICommand
    {
        public string Name { get; set; }
        public string? ParentId { get; set; }
    }
}
