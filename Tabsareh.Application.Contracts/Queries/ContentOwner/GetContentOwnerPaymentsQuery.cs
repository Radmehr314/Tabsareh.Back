using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.ContentOwner
{
    public class GetContentOwnerPaymentsQuery : IQuery
    {
        public string ContentOwnerId { get; set; }
    }
}
