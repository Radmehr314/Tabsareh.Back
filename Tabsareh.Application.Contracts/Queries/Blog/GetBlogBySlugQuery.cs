using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Blog
{
    public class GetBlogBySlugQuery : IQuery
    {
        public string Slug { get; set; }
    }
}
