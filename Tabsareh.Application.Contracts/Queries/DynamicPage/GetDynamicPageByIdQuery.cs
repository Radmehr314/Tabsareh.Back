using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.DynamicPage
{
    public class GetDynamicPageByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
