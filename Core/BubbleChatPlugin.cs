using BubbleChat.Config;
using BubbleChat.Commands;
using BubbleChat.Events;
using Exiled.API.Features;
using System;

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
        public override void OnEnabled()
        {
            Instance = this;
            _events = new PlayerEvents();
            Exiled.Events.Handlers.Player.Verified += _events.OnPlayerVerified;
            Exiled.Events.Handlers.Player.Died += _events.OnPlayerDied;
            PlayerBubbleManager.Instance.Enable();
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
    }
} 