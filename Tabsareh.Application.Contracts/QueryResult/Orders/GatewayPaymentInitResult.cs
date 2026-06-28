namespace Tabsareh.Application.Contracts.QueryResult.Orders
{
    public class GatewayPaymentInitResult
    {
        public string OrderId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

        /// <summary>آدرس صفحه پرداخت سامان — فرانت‌اند باید فرم POST به این آدرس ارسال کند</summary>
        public string PaymentUrl { get; set; } = "https://sep.shaparak.ir/OnlinePG/OnlinePG";
    }
}
