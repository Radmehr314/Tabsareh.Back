using Microsoft.AspNetCore.Mvc;

namespace Tabsareh.Api.Controllers.Gateway
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class GatewayCallbackController : Controller
    {
        [HttpGet("/gateway-test")]
        public IActionResult Test()
        {
            return Content(TestPageHtml(), "text/html", System.Text.Encoding.UTF8);
        }

        [HttpPost("/gateway-callback")]
        [HttpGet("/gateway-callback")]
        public IActionResult Callback()
        {
            var state     = Request.Form.TryGetValue("State",     out var s) ? s.ToString() : Request.Query["State"].ToString();
            var refNum    = Request.Form.TryGetValue("RefNum",    out var r) ? r.ToString() : Request.Query["RefNum"].ToString();
            var resNum    = Request.Form.TryGetValue("ResNum",    out var rn) ? rn.ToString() : Request.Query["ResNum"].ToString();
            var rrn       = Request.Form.TryGetValue("Rrn",       out var rv) ? rv.ToString() : Request.Query["Rrn"].ToString();
            var traceNo   = Request.Form.TryGetValue("TraceNo",   out var t) ? t.ToString() : Request.Query["TraceNo"].ToString();
            var securePan = Request.Form.TryGetValue("SecurePan", out var sp) ? sp.ToString() : Request.Query["SecurePan"].ToString();
            var mid       = Request.Form.TryGetValue("MID",       out var m) ? m.ToString() : Request.Query["MID"].ToString();
            var amount    = Request.Form.TryGetValue("Amount",    out var a) ? a.ToString() : Request.Query["Amount"].ToString();

            return Content(CallbackPageHtml(state, refNum, resNum, rrn, traceNo, securePan, mid, amount), "text/html", System.Text.Encoding.UTF8);
        }

        private static string TestPageHtml() => """
            <!DOCTYPE html>
            <html dir="rtl" lang="fa">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>تست درگاه پرداخت سامان</title>
                <style>
                    * { box-sizing: border-box; margin: 0; padding: 0; }
                    body { font-family: Tahoma, sans-serif; background: #f0f4f8; display: flex; justify-content: center; align-items: flex-start; min-height: 100vh; padding: 40px 16px; }
                    .card { background: white; border-radius: 12px; box-shadow: 0 2px 12px rgba(0,0,0,.1); padding: 32px; width: 100%; max-width: 520px; }
                    h2 { font-size: 20px; color: #1e293b; margin-bottom: 24px; text-align: center; }
                    label { display: block; font-size: 13px; color: #64748b; margin-bottom: 6px; margin-top: 16px; }
                    input, textarea { width: 100%; border: 1px solid #cbd5e1; border-radius: 8px; padding: 10px 12px; font-size: 14px; font-family: inherit; direction: ltr; }
                    textarea { height: 80px; resize: vertical; }
                    button { width: 100%; margin-top: 24px; background: #2563eb; color: white; border: none; border-radius: 8px; padding: 12px; font-size: 15px; font-family: inherit; cursor: pointer; }
                    button:hover { background: #1d4ed8; }
                    .hint { font-size: 12px; color: #94a3b8; margin-top: 8px; }
                    #result { margin-top: 20px; padding: 16px; border-radius: 8px; background: #f8fafc; border: 1px solid #e2e8f0; font-size: 13px; white-space: pre-wrap; direction: ltr; display: none; }
                    #result.error { background: #fef2f2; border-color: #fecaca; color: #b91c1c; }
                    #result.success { background: #f0fdf4; border-color: #bbf7d0; color: #15803d; }
                    .step { background: #eff6ff; border: 1px solid #bfdbfe; border-radius: 8px; padding: 12px 16px; margin-bottom: 16px; font-size: 13px; color: #1e40af; }
                    .step strong { display: block; margin-bottom: 4px; }
                </style>
            </head>
            <body>
            <div class="card">
                <h2>🧪 تست درگاه پرداخت سامان</h2>

                <div class="step">
                    <strong>مرحله ۱ — توکن بگیر</strong>
                    ابتدا با JWT کاربر، API مقابل را صدا بزن تا توکن بگیری:
                    <code>POST /InitiateGatewayPayment</code>
                </div>

                <label>توکن دریافتی از API</label>
                <input type="text" id="token" placeholder="توکن را اینجا بگذار..." />
                <p class="hint">بعد از صدا زدن /InitiateGatewayPayment، مقدار token را از جواب API اینجا بگذار</p>

                <button onclick="goToPay()">مرحله ۲ — رفتن به صفحه پرداخت سامان</button>

                <form id="sepForm" action="https://sep.shaparak.ir/OnlinePG/OnlinePG" method="POST" style="display:none">
                    <input type="hidden" name="Token" id="formToken" />
                    <input type="hidden" name="GetMethod" value="" />
                </form>

                <div id="result"></div>

                <script>
                    function goToPay() {
                        const token = document.getElementById('token').value.trim();
                        if (!token) { alert('لطفاً توکن را وارد کنید'); return; }
                        document.getElementById('formToken').value = token;
                        document.getElementById('sepForm').submit();
                    }
                </script>
            </div>
            </body>
            </html>
            """;

        private static string CallbackPageHtml(string state, string refNum, string resNum,
            string rrn, string traceNo, string securePan, string mid, string amount)
        {
            var isOk = state.Equals("OK", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(refNum);
            var statusColor = isOk ? "#15803d" : "#b91c1c";
            var statusBg = isOk ? "#f0fdf4" : "#fef2f2";
            var statusBorder = isOk ? "#bbf7d0" : "#fecaca";
            var statusText = isOk ? "✅ پرداخت موفق — در انتظار تأیید سرور" : $"❌ پرداخت ناموفق — وضعیت: {state}";

            return $$"""
                <!DOCTYPE html>
                <html dir="rtl" lang="fa">
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                    <title>نتیجه پرداخت</title>
                    <style>
                        * { box-sizing: border-box; margin: 0; padding: 0; }
                        body { font-family: Tahoma, sans-serif; background: #f0f4f8; display: flex; justify-content: center; align-items: flex-start; min-height: 100vh; padding: 40px 16px; }
                        .card { background: white; border-radius: 12px; box-shadow: 0 2px 12px rgba(0,0,0,.1); padding: 32px; width: 100%; max-width: 520px; }
                        h2 { font-size: 20px; color: #1e293b; margin-bottom: 24px; text-align: center; }
                        .status { padding: 16px; border-radius: 8px; text-align: center; font-size: 15px; font-weight: bold; margin-bottom: 24px; background: {{statusBg}}; border: 1px solid {{statusBorder}}; color: {{statusColor}}; }
                        table { width: 100%; border-collapse: collapse; font-size: 13px; }
                        td { padding: 10px 12px; border-bottom: 1px solid #f1f5f9; }
                        td:first-child { color: #64748b; width: 40%; }
                        td:last-child { font-family: monospace; direction: ltr; text-align: left; color: #1e293b; }
                        button { width: 100%; margin-top: 24px; background: #2563eb; color: white; border: none; border-radius: 8px; padding: 12px; font-size: 15px; font-family: inherit; cursor: pointer; }
                        button:hover { background: #1d4ed8; }
                        button:disabled { background: #94a3b8; cursor: default; }
                        #verifyResult { margin-top: 16px; padding: 16px; border-radius: 8px; font-size: 13px; white-space: pre-wrap; direction: ltr; display: none; }
                        #verifyResult.error { background: #fef2f2; border: 1px solid #fecaca; color: #b91c1c; }
                        #verifyResult.success { background: #f0fdf4; border: 1px solid #bbf7d0; color: #15803d; }
                        .api-url { font-size: 11px; color: #94a3b8; text-align: center; margin-top: 8px; }
                    </style>
                </head>
                <body>
                <div class="card">
                    <h2>نتیجه پرداخت درگاه سامان</h2>
                    <div class="status">{{statusText}}</div>

                    <table>
                        <tr><td>شماره سفارش (ResNum)</td><td>{{resNum}}</td></tr>
                        <tr><td>وضعیت (State)</td><td>{{state}}</td></tr>
                        <tr><td>شماره مرجع (RefNum)</td><td>{{(string.IsNullOrEmpty(refNum) ? "—" : refNum)}}</td></tr>
                        <tr><td>شماره پیگیری (RRN)</td><td>{{(string.IsNullOrEmpty(rrn) ? "—" : rrn)}}</td></tr>
                        <tr><td>شماره ترتیب (TraceNo)</td><td>{{(string.IsNullOrEmpty(traceNo) ? "—" : traceNo)}}</td></tr>
                        <tr><td>شماره کارت (SecurePan)</td><td>{{(string.IsNullOrEmpty(securePan) ? "—" : securePan)}}</td></tr>
                        <tr><td>شناسه پذیرنده (MID)</td><td>{{(string.IsNullOrEmpty(mid) ? "—" : mid)}}</td></tr>
                        <tr><td>مبلغ (ریال)</td><td>{{(string.IsNullOrEmpty(amount) ? "—" : amount)}}</td></tr>
                    </table>

                    <button id="verifyBtn" onclick="verify()">مرحله ۳ — تأیید پرداخت توسط سرور</button>
                    <p class="api-url">POST /VerifyGatewayPayment</p>
                    <div id="verifyResult"></div>

                    <script>
                        const data = {
                            resNum:    "{{resNum}}",
                            state:     "{{state}}",
                            refNum:    "{{refNum}}",
                            rRN:       "{{rrn}}",
                            traceNo:   "{{traceNo}}",
                            securePan: "{{securePan}}"
                        };

                        async function verify() {
                            const btn = document.getElementById('verifyBtn');
                            const result = document.getElementById('verifyResult');
                            btn.disabled = true;
                            btn.textContent = 'در حال تأیید...';
                            result.style.display = 'none';
                            try {
                                const resp = await fetch('/VerifyGatewayPayment', {
                                    method: 'POST',
                                    headers: { 'Content-Type': 'application/json' },
                                    body: JSON.stringify(data)
                                });
                                const json = await resp.json();
                                result.style.display = 'block';
                                if (resp.ok) {
                                    result.className = 'success';
                                    result.textContent = '✅ تأیید موفق!\n' + JSON.stringify(json, null, 2);
                                } else {
                                    result.className = 'error';
                                    result.textContent = '❌ خطا:\n' + JSON.stringify(json, null, 2);
                                }
                            } catch (e) {
                                result.style.display = 'block';
                                result.className = 'error';
                                result.textContent = '❌ خطای شبکه: ' + e.message;
                            }
                            btn.disabled = false;
                            btn.textContent = 'مرحله ۳ — تأیید پرداخت توسط سرور';
                        }
                    </script>
                </div>
                </body>
                </html>
                """;
        }
    }
}
