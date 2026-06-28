using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tabsareh.Application.Contracts.Contracts;

namespace Tabsareh.Infrastructure.Persistance.Services
{
    public class SepPaymentService : ISepPaymentService
    {
        private const string TokenUrl = "https://sep.shaparak.ir/onlinepg/onlinepg";
        private const string VerifyUrl = "https://sep.shaparak.ir/verifyTxnRandomSessionkey/ipg/VerifyTransaction";

        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        private static HttpClient CreateClient() => new();

        public async Task<SepTokenResult> GetTokenAsync(string terminalId, string resNum, long amount, string redirectUrl, string? cellNumber = null)
        {
            var payload = new Dictionary<string, object?>
            {
                ["action"] = "token",
                ["TerminalId"] = terminalId,
                ["Amount"] = amount,
                ["ResNum"] = resNum,
                ["RedirectUrl"] = redirectUrl
            };

            if (!string.IsNullOrWhiteSpace(cellNumber))
                payload["CellNumber"] = cellNumber;

            try
            {
                using var client = CreateClient();
                var response = await client.PostAsJsonAsync(TokenUrl, payload);
                var body = await response.Content.ReadAsStringAsync();
                var json = JsonSerializer.Deserialize<SepTokenApiResponse>(body, JsonOpts);

                if (json?.Status == 1 && !string.IsNullOrWhiteSpace(json.Token))
                    return new SepTokenResult { Success = true, Token = json.Token };

                return new SepTokenResult { Success = false, ErrorDesc = json?.ErrorDesc ?? "خطا در دریافت توکن از درگاه پرداخت" };
            }
            catch (Exception ex)
            {
                return new SepTokenResult { Success = false, ErrorDesc = $"خطا در اتصال به درگاه پرداخت: {ex.Message}" };
            }
        }

        public async Task<SepVerifyResult> VerifyAsync(long terminalNumber, string refNum)
        {
            var payload = new { RefNum = refNum, TerminalNumber = terminalNumber };

            try
            {
                using var client = CreateClient();
                var response = await client.PostAsJsonAsync(VerifyUrl, payload);
                var body = await response.Content.ReadAsStringAsync();
                var json = JsonSerializer.Deserialize<SepVerifyApiResponse>(body, JsonOpts);

                if (json is null)
                    return new SepVerifyResult { Success = false, ResultCode = -99, ResultDescription = "پاسخ نامعتبر از درگاه پرداخت" };

                return new SepVerifyResult
                {
                    Success = json.Success,
                    ResultCode = json.ResultCode,
                    ResultDescription = json.ResultDescription,
                    RRN = json.TransactionDetail?.RRN,
                    RefNum = json.TransactionDetail?.RefNum,
                    MaskedPan = json.TransactionDetail?.MaskedPan,
                    TraceNo = json.TransactionDetail?.StraceNo
                };
            }
            catch (Exception ex)
            {
                return new SepVerifyResult { Success = false, ResultCode = -99, ResultDescription = $"خطا در اتصال به درگاه پرداخت: {ex.Message}" };
            }
        }

        private sealed class SepTokenApiResponse
        {
            [JsonPropertyName("status")] public int Status { get; set; }
            [JsonPropertyName("token")] public string? Token { get; set; }
            [JsonPropertyName("errorCode")] public string? ErrorCode { get; set; }
            [JsonPropertyName("errorDesc")] public string? ErrorDesc { get; set; }
        }

        private sealed class SepVerifyApiResponse
        {
            [JsonPropertyName("success")] public bool Success { get; set; }
            [JsonPropertyName("resultCode")] public int ResultCode { get; set; }
            [JsonPropertyName("resultDescription")] public string? ResultDescription { get; set; }
            [JsonPropertyName("transactionDetail")] public SepTransactionDetail? TransactionDetail { get; set; }
        }

        private sealed class SepTransactionDetail
        {
            [JsonPropertyName("rRN")] public string? RRN { get; set; }
            [JsonPropertyName("refNum")] public string? RefNum { get; set; }
            [JsonPropertyName("maskedPan")] public string? MaskedPan { get; set; }
            [JsonPropertyName("straceNo")] public string? StraceNo { get; set; }
        }
    }
}
