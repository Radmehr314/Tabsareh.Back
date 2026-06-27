using Tabsareh.Application.Contracts.Commands.Cart;
using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Services;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Orders;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.CommandHandlers
{
    public class CartCommandHandler :
        ICommandHandler<AddToCartCommand>,
        ICommandHandler<RemoveFromCartCommand>,
        ICommandHandler<ClearCartCommand>,
        ICommandHandler<CreateOrderFromCartCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfoService _userInfoService;
        private readonly ILicenseProvisioningService _licenseProvisioningService;

        public CartCommandHandler(
            IUnitOfWork unitOfWork,
            IUserInfoService userInfoService,
            ILicenseProvisioningService licenseProvisioningService)
        {
            _unitOfWork = unitOfWork;
            _userInfoService = userInfoService;
            _licenseProvisioningService = licenseProvisioningService;
        }

        public async Task<CommandResult> Handle(AddToCartCommand command)
        {
            var userId = GetUserId();
            if (string.IsNullOrWhiteSpace(command.CourseId))
                throw new UserAccessException("شناسه دوره الزامی است.");

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(command.CourseId);
            if (course is null || course.IsDeleted || !course.IsActive)
                throw new NotFoundException("دوره یافت نشد.");

            var cart = await _unitOfWork.CartRepository.GetOrCreateAsync(userId);
            cart.AddItem(command.CourseId);
            await _unitOfWork.CartRepository.UpdateAsync(cart);
            return new CommandResult { Id = cart.Id };
        }

        public async Task<CommandResult> Handle(RemoveFromCartCommand command)
        {
            var userId = GetUserId();
            var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId);
            if (cart is null) return new CommandResult { Id = string.Empty };

            cart.RemoveItem(command.CourseId);
            await _unitOfWork.CartRepository.UpdateAsync(cart);
            return new CommandResult { Id = cart.Id };
        }

        public async Task<CommandResult> Handle(ClearCartCommand command)
        {
            var userId = GetUserId();
            var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId);
            if (cart is null) return new CommandResult { Id = string.Empty };

            cart.Clear();
            await _unitOfWork.CartRepository.UpdateAsync(cart);
            return new CommandResult { Id = cart.Id };
        }

        public async Task<CommandResult> Handle(CreateOrderFromCartCommand command)
        {
            var userId = GetUserId();

            if (command.PaymentMethod != OrderPaymentMethods.CardToCard && command.PaymentMethod != OrderPaymentMethods.Gateway)
                throw new UserAccessException("روش پرداخت نامعتبر است.");

            if (command.PaymentMethod == OrderPaymentMethods.CardToCard && string.IsNullOrWhiteSpace(command.CardToCardTrackingNumber))
                throw new UserAccessException("شماره پیگیری کارت به کارت الزامی است.");

            var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId);
            if (cart is null || cart.Items.Count == 0)
                throw new UserAccessException("سبد خرید شما خالی است.");

            var courseIds = cart.Items.Select(x => x.CourseId).ToList();
            var (invoice, courses, discountCode, licensePrice) = await OrderInvoiceBuilder.BuildAsync(
                _unitOfWork, courseIds, command.DiscountCode);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null || user.IsDeleted) throw new NotFoundException("کاربر یافت نشد.");

            var order = new Order(
                userId,
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
                    course.Price,
                    course.ContentOwnerSharePercent,
                    course.ContentOwnerId,
                    contentOwner?.Name ?? string.Empty));
            }

            discountCode?.Use();
            var id = await _unitOfWork.OrderRepository.AddAsync(order);

            // خالی کردن سبد بعد از ثبت سفارش
            cart.Clear();
            await _unitOfWork.CartRepository.UpdateAsync(cart);

            return new CommandResult { Id = id };
        }

        private string GetUserId()
        {
            var role = _userInfoService.GetRoleByToken();
            if (role != "user") throw new UserAccessException("فقط کاربران می‌توانند از سبد خرید استفاده کنند.");
            var userId = _userInfoService.GetUserIdByToken();
            if (string.IsNullOrWhiteSpace(userId)) throw new UserAccessException("کاربر نامعتبر است.");
            return userId;
        }
    }
}
