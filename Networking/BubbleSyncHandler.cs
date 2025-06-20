using Exiled.API.Features;
using Mirror;
using System.Linq;
using UnityEngine;

namespace BubbleChat.Networking
{
    public static class BubbleSyncHandler
    {
        public static void SendBubbleUpdate(Player player, string message, Vector3 position)
        {
            var msg = new BubbleSyncMessage
            {
                PlayerId = (ushort)player.Id,
                Message = message,
                Position = position
            };
            // Send to all clients for now (Mirror API)
            NetworkServer.SendToAll(msg);
        }
        public static void SendRemoveBubble(Player player)
        {
            var msg = new BubbleSyncMessage
            {
                PlayerId = (ushort)player.Id,
                Message = null,
                Position = Vector3.zero
            };
            NetworkServer.SendToAll(msg);
        }
    }
} 