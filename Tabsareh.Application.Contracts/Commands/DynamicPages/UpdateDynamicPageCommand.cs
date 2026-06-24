using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.DynamicPages
{
    public class UpdateDynamicPageCommand : ICommand
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Slug { get; set; }
        public string Body { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
    }
}
