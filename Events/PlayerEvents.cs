using BubbleChat.Core;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace BubbleChat.Events
{
    public class PlayerEvents
    {
        public void OnPlayerVerified(VerifiedEventArgs ev)
        {
            ev.Player.ShowHint("<color=yellow>💬  Chat enabled! Use .chat <message></color> in the client console to send messages!", 5);
        }
        public void OnPlayerDied(DiedEventArgs ev)
        {
            PlayerBubbleManager.Instance.RemoveBubble(ev.Player);
        }
    }
} 