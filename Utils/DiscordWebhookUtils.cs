using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using System;
using Newtonsoft.Json;

namespace BubbleChat.Utils
{
    public static class DiscordWebhookUtils
    {
        public static async Task SendToWebhookAsync(Player player, string message, string word, string category, string webhookUrl)
        {
            if (string.IsNullOrWhiteSpace(webhookUrl)) return;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var embed = new
                    {
                        title = "Blacklisted Word Detected",
                        description = $"A player has used a blacklisted word in chat.",
                        color = 15158332, // Red color
                        fields = new[]
                        {
                            new { name = "Player", value = $"{player.Nickname} ({player.UserId})", inline = true },
                            new { name = "Category", value = category, inline = true },
                            new { name = "Matched Word", value = word, inline = false },
                            new { name = "Full Message", value = message, inline = false }
                        },
                        timestamp = DateTime.UtcNow.ToString("o")
                    };

                    var payload = new { embeds = new[] { embed } };
                    var jsonPayload = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    await httpClient.PostAsync(webhookUrl, content);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to send Discord webhook alert: {ex}");
            }
        }
    }
} 