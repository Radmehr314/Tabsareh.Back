using Tabsareh.Application.Contracts.Commands.SiteSettings;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.SiteSettings;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class SiteSettingCommandHandler : ICommandHandler<UpdateLicensePriceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SiteSettingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(UpdateLicensePriceCommand command)
        {
            if (command.LicensePrice < 0)
                throw new UserAccessException("مبلغ لایسنس نمی‌تواند منفی باشد.");

            await _unitOfWork.SiteSettingRepository.SetAsync(
                SiteSettingKeys.LicensePrice,
                command.LicensePrice.ToString());

            return new CommandResult();
        }
    }
}
