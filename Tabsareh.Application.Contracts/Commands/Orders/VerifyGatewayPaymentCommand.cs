using Tabsareh.Framework.Application;

namespace Tabsareh.Application.Contracts.Commands.Orders
{
    public class VerifyGatewayPaymentCommand : ICommand
    {
        /// <summary>شماره سفارش ما — همان ResNum ارسالی به سامان</summary>
        public string ResNum { get; set; } = string.Empty;

        /// <summary>وضعیت بازگشتی از سامان: OK | CanceledByUser | Failed | ...</summary>
        public string State { get; set; } = string.Empty;

        /// <summary>شماره مرجع تراکنش از سامان (برای verify)</summary>
        public string? RefNum { get; set; }

        public string? RRN { get; set; }
        public string? TraceNo { get; set; }
        public string? SecurePan { get; set; }
    }
}
