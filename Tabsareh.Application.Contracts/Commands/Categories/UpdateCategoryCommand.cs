using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Categories
{
    public class UpdateCategoryCommand : ICommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ParentId { get; set; }
    }
}
