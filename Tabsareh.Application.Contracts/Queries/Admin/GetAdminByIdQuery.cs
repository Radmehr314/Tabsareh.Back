using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Queries.Admin;

public class GetAdminByIdQuery : IQuery
{
    public string Id { get; set; }
}
