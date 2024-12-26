using System.Net.Http.Headers;
using System.Text;

namespace MyApp.Services
{
    public class MailgunService
    {
        private readonly string _apiKey = "9b3866acdb682d157b35a8894d7004c4\r\n";
        private readonly string _domain = "sandboxeac19d9f630a4bc6b7822f7b490596ca.mailgun.org";
        private readonly string _baseUrl = "https://api.mailgun.net/v3";
        private readonly HttpClient _httpClient;

        public MailgunService()
        {
            // Создаем HttpClient
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new HttpClient();
            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"api:{_apiKey}"));

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("from", $"Your App <mailgun@{_domain}>"),
            new KeyValuePair<string, string>("to", to),
            new KeyValuePair<string, string>("subject", subject),
            new KeyValuePair<string, string>("html", body)
        });

            var response = await client.PostAsync($"https://api.mailgun.net/v3/{_domain}/messages", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
