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
        ICommandHandler<RejectCardToCardOrderCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILicenseProvisioningService _licenseProvisioningService;
        private readonly IUserInfoService _userInfoService;

        public OrderCommandHandler(
            IUnitOfWork unitOfWork,
            ILicenseProvisioningService licenseProvisioningService,
            IUserInfoService userInfoService)
        {
            _unitOfWork = unitOfWork;
            _licenseProvisioningService = licenseProvisioningService;
            _userInfoService = userInfoService;
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

            var (invoice, courses, discountCode) = await OrderInvoiceBuilder.BuildAsync(_unitOfWork, command.CourseIds, command.DiscountCode);

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
                    invoiceItem.FinalAmount,
                    course.SettlementBasePrice,
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
    }
}
