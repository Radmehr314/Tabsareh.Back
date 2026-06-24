using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.DynamicPage
{
    public class GetPublishedDynamicPageBySlugQuery : IQuery
    {
        public string Slug { get; set; }
    }
}
