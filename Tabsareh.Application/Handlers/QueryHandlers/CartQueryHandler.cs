using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Contracts.Queries.Cart;
using Tabsareh.Application.Contracts.QueryResult.Cart;
using Tabsareh.Application.Contracts.QueryResult.Orders;
using Tabsareh.Application.Services;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.SiteSettings;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class CartQueryHandler : IQueryHandler<GetMyCartQuery, CartResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfoService _userInfoService;

        public CartQueryHandler(IUnitOfWork unitOfWork, IUserInfoService userInfoService)
        {
            _unitOfWork = unitOfWork;
            _userInfoService = userInfoService;
        }

        public async Task<CartResult> Handle(GetMyCartQuery query)
        {
            var role = _userInfoService.GetRoleByToken();
            if (role != "user") throw new UserAccessException("فقط کاربران می‌توانند سبد خرید داشته باشند.");

            var userId = _userInfoService.GetUserIdByToken();
            if (string.IsNullOrWhiteSpace(userId)) throw new UserAccessException("کاربر نامعتبر است.");

            var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId);
            if (cart is null || cart.Items.Count == 0)
                return new CartResult();

            var licensePriceRaw = await _unitOfWork.SiteSettingRepository.GetAsync(SiteSettingKeys.LicensePrice);
            var licensePrice = decimal.TryParse(licensePriceRaw, out var lp) ? lp : 0m;

            var today = DateTime.Now.Date;
            var cartItems = new List<CartItemResult>();
            foreach (var item in cart.Items)
            {
                var course = await _unitOfWork.CourseRepository.GetByIdAsync(item.CourseId);
                if (course is null || course.IsDeleted || !course.IsActive) continue;

                var effectiveDiscount = GetActiveDiscountPercent(course, today);
                var discountedPrice = Math.Max(0, course.Price - Math.Round(course.Price * effectiveDiscount / 100));

                cartItems.Add(new CartItemResult
                {
                    CourseId = course.Id,
                    CourseTitle = course.Title,
                    BannerImage = course.BannerImage,
                    Price = course.Price,
                    LicensePrice = licensePrice,
                    TotalPrice = discountedPrice + licensePrice,
                    EffectiveDiscountPercent = effectiveDiscount > 0 ? effectiveDiscount : null
                });
            }

            // پیش‌فاکتور کامل
            OrderInvoiceResult? invoice = null;
            if (cartItems.Count > 0)
            {
                try
                {
                    var courseIds = cartItems.Select(x => x.CourseId).ToList();
                    var (inv, _, _, _) = await OrderInvoiceBuilder.BuildAsync(_unitOfWork, courseIds, null);
                    invoice = inv;
                }
                catch { }
            }

            return new CartResult { Items = cartItems, Invoice = invoice };
        }

        private static decimal GetActiveDiscountPercent(Tabsareh.Domain.Models.Courses.Course course, DateTime today)
        {
            if (!course.DiscountPercent.HasValue || course.DiscountPercent <= 0) return 0;
            if (course.DiscountStartDate.HasValue && today < course.DiscountStartDate.Value.Date) return 0;
            if (course.DiscountEndDate.HasValue && today > course.DiscountEndDate.Value.Date) return 0;
            return course.DiscountPercent.Value;
        }
    }
}
