using Microsoft.Extensions.Configuration;
using Tabsareh.Application.Contracts.Commands.Orders;
using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Services;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Orders;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class OrderCommandHandler :
        ICommandHandler<CreateOrderCommand>,
        ICommandHandler<ApproveCardToCardOrderCommand>,
        ICommandHandler<RejectCardToCardOrderCommand>,
        ICommandHandler<VerifyGatewayPaymentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILicenseProvisioningService _licenseProvisioningService;
        private readonly IUserInfoService _userInfoService;
        private readonly ISepPaymentService _sepPaymentService;
        private readonly IConfiguration _configuration;

        public OrderCommandHandler(
            IUnitOfWork unitOfWork,
            ILicenseProvisioningService licenseProvisioningService,
            IUserInfoService userInfoService,
            ISepPaymentService sepPaymentService,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _licenseProvisioningService = licenseProvisioningService;
            _userInfoService = userInfoService;
            _sepPaymentService = sepPaymentService;
            _configuration = configuration;
        }

        public async Task<CommandResult> Handle(CreateOrderCommand command)
        {
            if (command.PaymentMethod != OrderPaymentMethods.CardToCard && command.PaymentMethod != OrderPaymentMethods.Gateway)
                throw new UserAccessException("روش پرداخت نامعتبر است.");

            var tokenUserId = _userInfoService.GetUserIdByToken();
            var tokenRole = _userInfoService.GetRoleByToken();
            if (tokenRole != "user") throw new UserAccessException("فقط کاربران می‌توانند سفارش ثبت کنند.");
            if (string.IsNullOrWhiteSpace(tokenUserId)) throw new UserAccessException("کاربر نامعتبر است.");

            var user = await _unitOfWork.UserRepository.GetByIdAsync(tokenUserId);
            if (user is null || user.IsDeleted) throw new NotFoundException("کاربر یافت نشد.");

            if (command.PaymentMethod == OrderPaymentMethods.CardToCard && string.IsNullOrWhiteSpace(command.CardToCardTrackingNumber))
                throw new UserAccessException("شماره پیگیری کارت به کارت الزامی است.");

            var (invoice, courses, discountCode, licensePrice) = await OrderInvoiceBuilder.BuildAsync(_unitOfWork, command.CourseIds, command.DiscountCode);

            var order = new Order(
                tokenUserId,
                command.PaymentMethod,
                invoice.SubtotalAmount,
                invoice.CourseDiscountAmount,
                invoice.DiscountCodeAmount,
                invoice.PayableAmount,
                invoice.DiscountCode,
                invoice.DiscountCodePercent,
                command.CardToCardTrackingNumber,
                command.CardToCardDescription);

            order.SetId(Guid.NewGuid().ToString("N"));
            foreach (var invoiceItem in invoice.Items)
            {
                var course = courses.First(x => x.Id == invoiceItem.CourseId);
                var contentOwner = await _unitOfWork.ContentOwnerRepository.GetByIdAsync(course.ContentOwnerId);
                order.AddItem(new OrderItem(
                    order.Id,
                    course.Id,
                    course.Title,
                    invoiceItem.CoursePrice,
                    invoiceItem.CourseDiscountPercent,
                    invoiceItem.CourseDiscountAmount,
                    invoiceItem.DiscountCodePercent,
                    invoiceItem.DiscountCodeAmount,
                    licensePrice,
                    invoiceItem.FinalAmount,
                    Math.Max(0, invoiceItem.FinalAmount - licensePrice),
                    course.ContentOwnerSharePercent,
                    course.ContentOwnerId,
                    contentOwner?.Name ?? string.Empty));
            }

            discountCode?.Use();
            var id = await _unitOfWork.OrderRepository.AddAsync(order);
            return new CommandResult { Id = id };
        }

        public async Task<CommandResult> Handle(ApproveCardToCardOrderCommand command)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(command.OrderId);
            if (order is null) throw new NotFoundException("سفارش یافت نشد.");
            if (order.PaymentMethod != OrderPaymentMethods.CardToCard) throw new UserAccessException("این سفارش کارت به کارت نیست.");
            if (order.Status != OrderStatuses.PendingApproval) throw new UserAccessException("این سفارش در انتظار تأیید نیست.");

            foreach (var item in order.Items)
            {
                var licenseCode = await _licenseProvisioningService.CreateLicenseAsync(order.UserId, item.CourseId, order.Id);
                item.SetLicense(licenseCode);
            }

            order.Approve();
            await _unitOfWork.OrderRepository.UpdateAsync(order);
            return new CommandResult { Id = order.Id };
        }

        public async Task<CommandResult> Handle(RejectCardToCardOrderCommand command)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(command.OrderId);
            if (order is null) throw new NotFoundException("سفارش یافت نشد.");
            if (order.PaymentMethod != OrderPaymentMethods.CardToCard) throw new UserAccessException("این سفارش کارت به کارت نیست.");
            if (order.Status != OrderStatuses.PendingApproval) throw new UserAccessException("این سفارش در انتظار تأیید نیست.");

            order.Reject(command.Reason);
            await _unitOfWork.OrderRepository.UpdateAsync(order);
            return new CommandResult { Id = order.Id };
        }

        public async Task<CommandResult> Handle(VerifyGatewayPaymentCommand command)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(command.ResNum);
            if (order is null) throw new NotFoundException("سفارش یافت نشد.");
            if (order.PaymentMethod != OrderPaymentMethods.Gateway) throw new UserAccessException("این سفارش از نوع درگاه پرداخت نیست.");
            if (order.Status != OrderStatuses.PendingPayment) throw new UserAccessException("وضعیت سفارش برای تأیید پرداخت معتبر نیست.");

            if (!command.State.Equals("OK", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(command.RefNum))
            {
                order.Reject(command.State);
                await _unitOfWork.OrderRepository.UpdateAsync(order);
                return new CommandResult { Id = order.Id };
            }

            var terminalId = _configuration["Sep:TerminalId"]
                ?? throw new InvalidOperationException("Sep:TerminalId is not configured.");

            var verifyResult = await _sepPaymentService.VerifyAsync(long.Parse(terminalId), command.RefNum);

            if (!verifyResult.Success)
            {
                order.Reject(verifyResult.ResultDescription ?? "خطا در تأیید پرداخت");
                await _unitOfWork.OrderRepository.UpdateAsync(order);
                return new CommandResult { Id = order.Id };
            }

            foreach (var item in order.Items)
            {
                var licenseCode = await _licenseProvisioningService.CreateLicenseAsync(order.UserId, item.CourseId, order.Id);
                item.SetLicense(licenseCode);
            }

            order.CompleteGatewayPayment(
                command.RefNum,
                verifyResult.RRN ?? command.RRN,
                verifyResult.TraceNo ?? command.TraceNo,
                verifyResult.MaskedPan ?? command.SecurePan);

            await _unitOfWork.OrderRepository.UpdateAsync(order);
            return new CommandResult { Id = order.Id };
        }
    }
}
