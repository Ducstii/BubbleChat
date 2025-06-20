using BubbleChat.Config;
using BubbleChat.Commands;
using BubbleChat.Events;
using Exiled.API.Features;
using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace BubbleChat.Core
{
    public class BubbleChatPlugin : Plugin<PluginConfig>
    {
        public override string Name => "BubbleChat";
        public override string Author => "Ducstii";
        public override Version Version => new Version(1, 0, 0);
        public override string Prefix => "BubbleChat";
        public static BubbleChatPlugin Instance { get; private set; }
        private PlayerEvents _events;
        public static readonly DateTime CurrentReleaseDate = new DateTime(2024, 6, 20, 0, 0, 0, DateTimeKind.Utc); // Set to your build date
        public override void OnEnabled()
        {
            Instance = this;
            _events = new PlayerEvents();
            Exiled.Events.Handlers.Player.Verified += _events.OnPlayerVerified;
            Exiled.Events.Handlers.Player.Died += _events.OnPlayerDied;
            PlayerBubbleManager.Instance.Enable();
            PlayerBubbleManager.Instance.RefreshBlacklistFromConfig();
            CheckForUpdates();
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Verified -= _events.OnPlayerVerified;
            Exiled.Events.Handlers.Player.Died -= _events.OnPlayerDied;
            PlayerBubbleManager.Instance.Disable();
            Instance = null;
            base.OnDisabled();
        }
        private void CheckForUpdates()
        {
            try
            {
                string apiUrl = "https://api.github.com/repos/Ducstii/BubbleChat/releases";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.UserAgent = "BubbleChat";

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new System.IO.StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    var releases = JArray.Parse(json);

                    foreach (var release in releases)
                    {
                        bool isPrerelease = release["prerelease"]?.ToObject<bool>() ?? false;
                        if (!isPrerelease)
                        {
                            string tag = release["tag_name"]?.ToString();
                            string publishedAt = release["published_at"]?.ToString();
                            DateTime publishedDate = DateTime.Parse(publishedAt).ToUniversalTime();

                            if (publishedDate > CurrentReleaseDate)
                            {
                                Log.Warn($"A new version of BubbleChat is available: {tag} (Published {publishedDate:yyyy-MM-dd}). You are running a version from {CurrentReleaseDate:yyyy-MM-dd}.");
                                Log.Warn("Download it at: https://github.com/Ducstii/BubbleChat/releases/latest");
                            }
                            else
                            {
                                Log.Info("BubbleChat is up to date.");
                            }
                            break; // Only check the latest non-prerelease
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Could not check for updates: {ex.Message}");
            }
        }
    }
} 