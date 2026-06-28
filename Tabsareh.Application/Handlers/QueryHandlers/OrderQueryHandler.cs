using Tabsareh.Application.Contracts.Contracts;
using Tabsareh.Application.Contracts.Queries.Orders;
using Tabsareh.Application.Contracts.QueryResult.Orders;
using Tabsareh.Application.Mapper;
using Tabsareh.Application.Services;
using Tabsareh.Domain;
using Tabsareh.Domain.Models.Orders;
using Tabsareh.Framework.Application;
using Tabsareh.Framework.Application.Exceptions;

namespace Tabsareh.Application.Handlers.QueryHandlers
{
    public class OrderQueryHandler :
        IQueryHandler<GetOrdersPagedQuery, GetOrdersPagedQueryResult>,
        IQueryHandler<GetPendingCardToCardOrdersQuery, GetOrdersPagedQueryResult>,
        IQueryHandler<PreviewOrderInvoiceQuery, OrderInvoiceResult>,
        IQueryHandler<GetMyOrdersQuery, List<OrderItemResult>>,
        IQueryHandler<GetMyOrdersAsContentOwnerQuery, GetOrdersPagedQueryResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfoService _userInfoService;

        public OrderQueryHandler(IUnitOfWork unitOfWork, IUserInfoService userInfoService)
        {
            _unitOfWork = unitOfWork;
            _userInfoService = userInfoService;
        }

        public async Task<GetOrdersPagedQueryResult> Handle(GetOrdersPagedQuery query)
        {
            var paged = await _unitOfWork.OrderRepository.GetPagedAsync(query.Options, new OrderFilter
            {
                Search = query.Search,
                Status = query.Status,
                PaymentMethod = query.PaymentMethod,
                ContentOwnerId = query.ContentOwnerId,
                FromDate = query.FromDate,
                ToDate = query.ToDate
            });

            return ToPagedResult(paged);
        }

        public async Task<GetOrdersPagedQueryResult> Handle(GetPendingCardToCardOrdersQuery query)
        {
            var paged = await _unitOfWork.OrderRepository.GetPagedAsync(query.Options, new OrderFilter
            {
                Search = query.Search,
                Status = OrderStatuses.PendingApproval,
                PaymentMethod = OrderPaymentMethods.CardToCard
            });

            return ToPagedResult(paged);
        }

        public async Task<List<OrderItemResult>> Handle(GetMyOrdersQuery query)
        {
            var userId = _userInfoService.GetUserIdByToken();
            if (string.IsNullOrWhiteSpace(userId)) throw new UserAccessException("کاربر نامعتبر است.");
            var orders = await _unitOfWork.OrderRepository.GetByUserIdAsync(userId);
            return orders.Select(x => x.ToItem()).ToList();
        }

        public async Task<GetOrdersPagedQueryResult> Handle(GetMyOrdersAsContentOwnerQuery query)
        {
            var ownerId = _userInfoService.GetUserIdByToken();
            var paged = await _unitOfWork.OrderRepository.GetPagedAsync(query.Options, new OrderFilter
            {
                Search = query.Search,
                Status = query.Status,
                ContentOwnerId = ownerId
            });

            return new GetOrdersPagedQueryResult
            {
                Items = paged.Items.Select(x => x.ToItem(maskPhone: true)).ToList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }

        public async Task<OrderInvoiceResult> Handle(PreviewOrderInvoiceQuery query)
        {
            var role = _userInfoService.GetRoleByToken();
            if (role != "user") throw new UserAccessException("فقط کاربران می‌توانند پیش‌فاکتور مشاهده کنند.");

            var userId = _userInfoService.GetUserIdByToken();
            var cart = await _unitOfWork.CartRepository.GetByUserIdAsync(userId);
            if (cart is null || cart.Items.Count == 0)
                throw new UserAccessException("سبد خرید شما خالی است.");

            var courseIds = cart.Items.Select(x => x.CourseId).ToList();
            var (invoice, _, _, _) = await OrderInvoiceBuilder.BuildAsync(_unitOfWork, courseIds, query.DiscountCode);
            return invoice;
        }

        private static GetOrdersPagedQueryResult ToPagedResult(Domain.Common.PagedResult<OrderListItem> paged)
        {
            return new GetOrdersPagedQueryResult
            {
                Items = paged.Items.Select(x => x.ToItem()).ToList(),
                TotalCount = (int)paged.TotalCount,
                Skip = paged.Skip,
                Limit = paged.Limit
            };
        }
    }
}
