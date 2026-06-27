using Tabsareh.Application.Contracts.Queries.SiteSettings;
using Tabsareh.Application.Contracts.QueryResult.SiteSettings;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.SiteSettings;
using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class SiteSettingQueryHandler : IQueryHandler<GetSiteSettingsQuery, SiteSettingsResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SiteSettingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SiteSettingsResult> Handle(GetSiteSettingsQuery query)
        {
            var raw = await _unitOfWork.SiteSettingRepository.GetAsync(SiteSettingKeys.LicensePrice);
            var licensePrice = decimal.TryParse(raw, out var lp) ? lp : 0m;

            return new SiteSettingsResult { LicensePrice = licensePrice };
        }
    }
}
