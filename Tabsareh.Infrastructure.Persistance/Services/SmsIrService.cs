using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Tabsareh.Application.Contracts.Contracts;

namespace Tabsareh.Infrastructure.Persistance.Services
{
    public class SmsIrService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsIrService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendOtpAsync(string mobile, string code)
        {
            try
            {
                var apiKey = _configuration["SmsIr:ApiKey"];
                var templateId = int.Parse(_configuration["SmsIr:TemplateId"]!);

                var payload = new
                {
                    mobile,
                    templateId,
                    parameters = new[]
                    {
                        new { name = "VERIFICATIONCODE", value = code }
                    }
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await httpClient.PostAsync("https://api.sms.ir/v1/send/verify", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
