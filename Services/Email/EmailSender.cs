using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BrixCMS.Open.Services.Email
{
    public class EmailSender
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public EmailSender(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        // ── Plain text ────────────────────────────────────────────────────────
        public Task SendEmailAsync(string senderName, string senderEmail, string toName, string toEmail, string subject, string textContent)
            => SendViaResendAsync(senderName, toEmail, subject, textContent, null);

        // ── HTML ──────────────────────────────────────────────────────────────
        public Task SendHtmlEmailAsync(string senderName, string senderEmail, string toName, string toEmail, string subject, string htmlContent, string? plainText = null)
            => SendViaResendAsync(senderName, toEmail, subject, plainText ?? StripHtml(htmlContent), htmlContent);

        // ── Sync wrappers (kept for compatibility) ────────────────────────────
        public void SendEmail(string senderName, string senderEmail, string toName, string toEmail, string subject, string textContent)
            => SendEmailAsync(senderName, senderEmail, toName, toEmail, subject, textContent).GetAwaiter().GetResult();

        public void SendHtmlEmail(string senderName, string senderEmail, string toName, string toEmail, string subject, string htmlContent, string? plainText = null)
            => SendHtmlEmailAsync(senderName, senderEmail, toName, toEmail, subject, htmlContent, plainText).GetAwaiter().GetResult();

        // ── Resend HTTP API ───────────────────────────────────────────────────
        private async Task SendViaResendAsync(string senderName, string toEmail, string subject, string? text, string? html)
        {
            var apiKey  = _config.GetValue<string>("SmtpSettings:SmtpPassword") ?? "";
            var from    = _config.GetValue<string>("SmtpSettings:FromEmail") ?? "onboarding@resend.dev";

            var payload = new Dictionary<string, object>
            {
                ["from"]    = $"{senderName} <{from}>",
                ["to"]      = new[] { toEmail },
                ["subject"] = subject,
            };
            if (!string.IsNullOrEmpty(html))  payload["html"] = html;
            if (!string.IsNullOrEmpty(text))  payload["text"] = text;

            var client  = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            var body     = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[EmailSender] Resend error {(int)response.StatusCode}: {body}");
                throw new Exception($"Resend error {(int)response.StatusCode}: {body}");
            }

            Console.WriteLine($"[EmailSender] Email enviado correctamente a {toEmail}");
        }

        private static string StripHtml(string html)
            => System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "").Trim();
    }
}
