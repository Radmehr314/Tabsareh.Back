using Tabsareh.Framework.Application;
using Tabsareh.Domain.Common;

namespace Tabsareh.Application.Contracts.Queries.Admin;

public class GetAllAdminQuery : IQuery
{
    public QueryOptions Options { get; set; } = new();
}
