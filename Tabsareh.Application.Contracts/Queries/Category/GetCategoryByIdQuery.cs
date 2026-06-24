using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Category
{
    public class GetCategoryByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
