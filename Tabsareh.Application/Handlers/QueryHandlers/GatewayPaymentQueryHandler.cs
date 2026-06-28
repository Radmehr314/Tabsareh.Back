using Microsoft.Extensions.Configuration;
using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Contracts.Queries.Orders;
using Tabsareh.Application.Contracts.QueryResult.Orders;
using Tabsareh.Application.Services;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Orders;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class GatewayPaymentQueryHandler : IQueryHandler<InitiateGatewayPaymentQuery, GatewayPaymentInitResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfoService _userInfoService;
        private readonly ISepPaymentService _sepPaymentService;
        private readonly IConfiguration _configuration;

        public GatewayPaymentQueryHandler(
            IUnitOfWork unitOfWork,
            IUserInfoService userInfoService,
            ISepPaymentService sepPaymentService,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userInfoService = userInfoService;
            _sepPaymentService = sepPaymentService;
            _configuration = configuration;
        }

        public async Task<GatewayPaymentInitResult> Handle(InitiateGatewayPaymentQuery query)
        {
            var tokenUserId = _userInfoService.GetUserIdByToken();
            var tokenRole = _userInfoService.GetRoleByToken();
            if (tokenRole != "user") throw new UserAccessException("فقط کاربران می‌توانند سفارش ثبت کنند.");
            if (string.IsNullOrWhiteSpace(tokenUserId)) throw new UserAccessException("کاربر نامعتبر است.");

            var user = await _unitOfWork.UserRepository.GetByIdAsync(tokenUserId);
            if (user is null || user.IsDeleted) throw new NotFoundException("کاربر یافت نشد.");

            var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(tokenUserId);
            if (cart is null || cart.Items.Count == 0)
                throw new UserAccessException("سبد خرید شما خالی است.");

            var courseIds = cart.Items.Select(x => x.CourseId).ToList();
            var (invoice, courses, discountCode, licensePrice) = await OrderInvoiceBuilder.BuildAsync(_unitOfWork, courseIds, query.DiscountCode);

            var order = new Order(
                tokenUserId,
                OrderPaymentMethods.Gateway,
                invoice.SubtotalAmount,
                invoice.CourseDiscountAmount,
                invoice.DiscountCodeAmount,
                invoice.PayableAmount,
                invoice.DiscountCode,
                invoice.DiscountCodePercent,
                cardToCardTrackingNumber: null,
                cardToCardDescription: null);

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
            await _unitOfWork.OrderRepository.AddAsync(order);

            cart.Clear();
            await _unitOfWork.CartRepository.UpdateAsync(cart);

            var terminalId = _configuration["Sep:TerminalId"]
                ?? throw new InvalidOperationException("Sep:TerminalId is not configured.");
            var redirectUrl = _configuration["Sep:RedirectUrl"]
                ?? throw new InvalidOperationException("Sep:RedirectUrl is not configured.");

            var amountRials = (long)Math.Round(order.PayableAmount * 10);
            var tokenResult = await _sepPaymentService.GetTokenAsync(terminalId, order.Id, amountRials, redirectUrl, query.CellNumber);

            if (!tokenResult.Success || string.IsNullOrWhiteSpace(tokenResult.Token))
                throw new UserAccessException(tokenResult.ErrorDesc ?? "خطا در دریافت توکن از درگاه پرداخت");

            order.SetGatewayToken(tokenResult.Token);
            await _unitOfWork.OrderRepository.UpdateAsync(order);

            return new GatewayPaymentInitResult
            {
                OrderId = order.Id,
                Token = tokenResult.Token,
                PaymentUrl = "https://sep.shaparak.ir/OnlinePG/OnlinePG"
            };
        }
    }
}
