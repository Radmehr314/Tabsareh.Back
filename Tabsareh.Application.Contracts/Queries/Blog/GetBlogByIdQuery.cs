using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Blog
{
    public class GetBlogByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
