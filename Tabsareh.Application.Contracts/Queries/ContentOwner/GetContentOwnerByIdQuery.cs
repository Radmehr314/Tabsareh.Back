using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.ContentOwner
{
    public class GetContentOwnerByIdQuery : IQuery
    {
        public string Id { get; set; }
    }
}
