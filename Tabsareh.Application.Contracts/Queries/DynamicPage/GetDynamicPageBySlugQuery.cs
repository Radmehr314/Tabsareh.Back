using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.DynamicPage
{
    public class GetDynamicPageBySlugQuery : IQuery
    {
        public string Slug { get; set; }
    }
}
