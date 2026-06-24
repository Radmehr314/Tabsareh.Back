using Tabsareh.Domain.Common;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Blog
{
    public class GetBlogsPagedQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
    }
}
