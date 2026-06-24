using Tabsareh.Domain.Common;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Category
{
    public class GetCategoriesPagedQuery : IQuery
    {
        public QueryOptions Options { get; set; } = new();
    }
}
