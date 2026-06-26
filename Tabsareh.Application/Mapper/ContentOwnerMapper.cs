using Tabsareh.Application.Contracts.QueryResult.ContentOwner;
using Tabsareh.Domain.Models.ContentOwners;
using Tabsareh.Framework.Application.Convertor;

namespace Tabsareh.Application.Mapper;

public static class ContentOwnerMapper
{
    public static ContentOwnerItemResult ToItem(this ContentOwner owner, decimal debtAmount = 0, decimal paidAmount = 0)
    {
        if (owner == null)
            throw new ArgumentNullException(nameof(owner));

        return new ContentOwnerItemResult
        {
            Id = owner.Id,
            Name = owner.Name,
            UserName = owner.UserName,
            IsBan = owner.IsBan,
            DebtAmount = debtAmount,
            PaidAmount = paidAmount,
            CreatedAt = owner.CreatedAt.ToShamsiDate()
        };
    }

    public static ContentOwnerPaymentItemResult ToPaymentItem(this ContentOwnerPayment payment, ContentOwner owner)
    {
        return new ContentOwnerPaymentItemResult
        {
            Id = payment.Id,
            ContentOwnerId = payment.ContentOwnerId,
            ContentOwnerName = owner.Name,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentDatePersian = payment.PaymentDate.ToShamsiDate(),
            ReceiptImage = payment.ReceiptImage,
            TrackingNumber = payment.TrackingNumber,
            Description = payment.Description,
            CreatedAt = payment.CreatedAt.ToShamsiDate()
        };
    }
}
