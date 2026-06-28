namespace Tabsareh.Application.Contracts.Contracts
{
    public interface ISepPaymentService
    {
        Task<SepTokenResult> GetTokenAsync(string terminalId, string resNum, long amount, string redirectUrl, string? cellNumber = null);
        Task<SepVerifyResult> VerifyAsync(long terminalNumber, string refNum);
    }

    public class SepTokenResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? ErrorDesc { get; set; }
    }

    public class SepVerifyResult
    {
        public bool Success { get; set; }
        public int ResultCode { get; set; }
        public string? ResultDescription { get; set; }
        public string? RRN { get; set; }
        public string? RefNum { get; set; }
        public string? MaskedPan { get; set; }
        public string? TraceNo { get; set; }
    }
}
