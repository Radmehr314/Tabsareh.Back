using Tabsareh.Application.Contracts.QueryResult.Orders;
using Tabsareh.Domain.Models.Orders;
using Tabsareh.Framework.Application.Convertor;

namespace Tabsareh.Application.Mapper
{
    public static class OrderMapper
    {
        public static OrderItemResult ToItem(this OrderListItem item)
        {
            var order = item.Order;
            return new OrderItemResult
            {
                Id = order.Id,
                UserId = order.UserId,
                UserPhone = item.UserPhone,
                UserFullName = item.UserFullName,
                CourseId = order.CourseId,
                CourseTitle = order.Items.Count == 1
                    ? order.Items.First().CourseTitleSnapshot
                    : $"{order.Items.Count} دوره",
                ContentOwnerId = SingleContentOwnerId(order),
                ContentOwnerName = ContentOwnerTitle(order),
                PaymentMethod = order.PaymentMethod,
                PaymentMethodTitle = PaymentMethodTitle(order.PaymentMethod),
                Status = order.Status,
                StatusTitle = StatusTitle(order.Status),
                CoursePrice = order.CoursePrice,
                DiscountPercentSnapshot = order.DiscountPercentSnapshot,
                Amount = order.PayableAmount > 0 ? order.PayableAmount : order.Amount,
                SettlementBasePriceSnapshot = order.SettlementBasePriceSnapshot,
                ContentOwnerSharePercentSnapshot = order.ContentOwnerSharePercentSnapshot,
                SubtotalAmount = order.SubtotalAmount,
                CourseDiscountAmount = order.CourseDiscountAmount,
                DiscountCode = order.DiscountCode,
                DiscountCodePercentSnapshot = order.DiscountCodePercentSnapshot,
                DiscountCodeAmount = order.DiscountCodeAmount,
                PayableAmount = order.PayableAmount,
                CreatedAt = order.CreatedAt.ToShamsiDate(),
                PaidAt = order.PaidAt?.ToShamsiDate(),
                CardToCardTrackingNumber = order.CardToCardTrackingNumber,
                CardToCardDescription = order.CardToCardDescription,
                RejectionReason = order.RejectionReason,
                LicenseCode = order.LicenseCode,
                Items = order.Items.Select(x => new OrderDetailItemResult
                {
                    CourseId = x.CourseId,
                    CourseTitle = x.CourseTitleSnapshot,
                    CoursePrice = x.CoursePriceSnapshot,
                    CourseDiscountPercent = x.CourseDiscountPercentSnapshot,
                    CourseDiscountAmount = x.CourseDiscountAmountSnapshot,
                    DiscountCodePercent = x.DiscountCodePercentSnapshot,
                    DiscountCodeAmount = x.DiscountCodeAmountSnapshot,
                    FinalAmount = x.FinalAmount,
                    ContentOwnerId = x.ContentOwnerIdSnapshot,
                    ContentOwnerName = x.ContentOwnerNameSnapshot,
                    LicenseCode = x.LicenseCode
                }).ToList()
            };
        }

        private static string? SingleContentOwnerId(Order order)
        {
            var ids = order.Items
                .Select(x => x.ContentOwnerIdSnapshot)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();
            return ids.Count == 1 ? ids[0] : null;
        }

        private static string ContentOwnerTitle(Order order)
        {
            var names = order.Items
                .Select(x => x.ContentOwnerNameSnapshot)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();
            if (names.Count == 0) return "-";
            return names.Count == 1 ? names[0] : "چند صاحب اثر";
        }

        private static string PaymentMethodTitle(string method) => method switch
        {
            OrderPaymentMethods.CardToCard => "کارت به کارت",
            OrderPaymentMethods.Gateway => "درگاه پرداخت",
            _ => method
        };

        private static string StatusTitle(string status) => status switch
        {
            OrderStatuses.PendingPayment => "منتظر پرداخت",
            OrderStatuses.PendingApproval => "منتظر تایید",
            OrderStatuses.Success => "موفق",
            OrderStatuses.Canceled => "لغو شده",
            _ => status
        };
    }
}
