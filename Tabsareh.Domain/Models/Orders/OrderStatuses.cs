namespace Tabsareh.Domain.Models.Orders
{
    public static class OrderStatuses
    {
        public const string PendingPayment = "PendingPayment";
        public const string PendingApproval = "PendingApproval";
        public const string Success = "Success";
        public const string Canceled = "Canceled";
    }
}
