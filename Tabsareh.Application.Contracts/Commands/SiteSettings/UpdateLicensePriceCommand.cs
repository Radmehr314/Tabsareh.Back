using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.SiteSettings
{
    public class UpdateLicensePriceCommand : ICommand
    {
        public decimal LicensePrice { get; set; }
    }
}
