using Tabsareh.Application.Contracts.Commands.Discounts;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Discounts;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class DiscountCodeCommandHandler :
        ICommandHandler<AddDiscountCodeCommand>,
        ICommandHandler<UpdateDiscountCodeCommand>,
        ICommandHandler<DeleteDiscountCodeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountCodeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(AddDiscountCodeCommand command)
        {
            await Validate(command.Title, command.Code, command.UsageLimit, command.DiscountPercent, command.StartDate, command.EndDate);
            var discountCode = new DiscountCode(command.Title.Trim(), command.Code.Trim(), command.UsageLimit, command.DiscountPercent, command.StartDate, command.EndDate);
            discountCode.SetId(Guid.NewGuid().ToString("N"));
            var id = await _unitOfWork.DiscountCodeRepository.AddAsync(discountCode);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(UpdateDiscountCodeCommand command)
        {
            await Validate(command.Title, command.Code, command.UsageLimit, command.DiscountPercent, command.StartDate, command.EndDate, command.Id);
            var discountCode = await _unitOfWork.DiscountCodeRepository.GetByIdAsync(command.Id);
            if (discountCode is null || discountCode.IsDeleted) throw new NotFoundException("کد تخفیف یافت نشد.");
            discountCode.Update(command.Title.Trim(), command.Code.Trim(), command.UsageLimit, command.DiscountPercent, command.StartDate, command.EndDate);
            await _unitOfWork.DiscountCodeRepository.UpdateAsync(discountCode);
            return new CommandResult { Id = discountCode.Id };
        }

        public async Task<CommandResult> Handle(DeleteDiscountCodeCommand command)
        {
            var discountCode = await _unitOfWork.DiscountCodeRepository.GetByIdAsync(command.Id);
            if (discountCode is null || discountCode.IsDeleted) throw new NotFoundException("کد تخفیف یافت نشد.");
            discountCode.Delete();
            await _unitOfWork.DiscountCodeRepository.UpdateAsync(discountCode);
            return new CommandResult { Id = discountCode.Id };
        }

        private async Task Validate(string title, string code, int usageLimit, decimal discountPercent, DateTime startDate, DateTime endDate, string? ignoreId = null)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new UserAccessException("عنوان کد تخفیف الزامی است.");
            if (string.IsNullOrWhiteSpace(code)) throw new UserAccessException("کد تخفیف الزامی است.");
            if (usageLimit <= 0) throw new UserAccessException("تعداد مصرف باید بزرگ‌تر از صفر باشد.");
            if (discountPercent <= 0 || discountPercent > 100) throw new UserAccessException("درصد تخفیف باید بین ۱ تا ۱۰۰ باشد.");
            if (startDate.Date > endDate.Date) throw new UserAccessException("تاریخ شروع نمی‌تواند بعد از تاریخ پایان باشد.");
            if (await _unitOfWork.DiscountCodeRepository.ExistsByCodeAsync(code, ignoreId)) throw new UserAccessException("این کد تخفیف قبلا ثبت شده است.");
        }
    }
}
