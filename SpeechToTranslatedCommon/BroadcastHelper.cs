
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Net.Http.Json;

namespace TranslateWordsConsole
{
    public class BroadcastHelper
    {
        private HttpClient client;
        private readonly IConfiguration configuration;
        private readonly string countryCode;

        public BroadcastHelper(IConfiguration configuration, string countryCode)
        {
            this.configuration = configuration;
            this.countryCode = countryCode;

            if (IsBroadcastService)
                client = new HttpClient { BaseAddress = new Uri(configuration["online.broadcast.url"]?.ToString() ?? "") };
        }

        public bool IsBroadcastService => !string.IsNullOrWhiteSpace(configuration["online.broadcast.url"]);

        public async Task WakeupAsync()
        {
            bool isOk = false;
            int count = 1;
            var retryCount = int.Parse(configuration?["online.broadcast.retry.count"] ?? "3");
            var retryDelay = int.Parse(configuration?["online.broadcast.retry.delayms"] ?? "5000");
            while (!isOk)
            {
                using HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, configuration?["online.broadcast.url"]);
                var url = new Uri(new Uri(configuration?["online.broadcast.url"] ?? throw new InvalidOperationException("online.broadcast.url")), @"\wakeup");
                var resp = await client.GetAsync(url);
                isOk = resp.StatusCode == System.Net.HttpStatusCode.OK;
                
                if (count > retryCount)
                    throw new InvalidOperationException("wakup took too long");

                if (!isOk)
                    await Task.Delay(retryDelay * count++);
            }
        }

        public void BroadcastFinalParagraph(string translation, Color colour)
        {
            var str = "#" + colour.R.ToString("X2") + colour.G.ToString("X2") + colour.B.ToString("X2");
            using HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, configuration["online.broadcast.url"]);
            message.Content = JsonContent.Create(new { translation, colour = str });

            using HttpContent content = message.Content;
            var resp = client.PostAsync($"/newtranslation/{countryCode}", content).Result;
        }

        public void BroadcastIncremental(string words, string translation)
        {
            using HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, configuration["online.broadcast.url"]);
            message.Content = JsonContent.Create(new { words, translation });

            using HttpContent content = message.Content;
            var resp = client.PostAsync($"/newincremental/{countryCode}", content).Result;
        }

        public void BroadcastAbsolute(string words, string translation)
        {
            using HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, configuration["online.broadcast.url"]);
            message.Content = JsonContent.Create(new { words, translation });

            using HttpContent content = message.Content;
            var resp = client.PostAsync($"/newabsolute/{countryCode}", content).Result;
        }
    }
}