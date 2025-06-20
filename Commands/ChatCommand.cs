using CommandSystem;
using RemoteAdmin;
using BubbleChat.Config;
using BubbleChat.Core;
using BubbleChat.Networking;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BubbleChat.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ChatCommand : ICommand
    {
        public string Command => "chat";
        public string[] Aliases => new[] { "c", "say" };
        public string Description => "Send a proximity chat bubble message";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "An error occurred!";
            if (!(sender is PlayerCommandSender pcs) || Player.Get(pcs.ReferenceHub) is not Player player)
            {
                response = "You must be a player to use this command.";
                return false;
            }
            if (!player.IsAlive)
            {
                response = "You must be alive to use chat bubbles.";
                return false;
            }
            var config = BubbleChatPlugin.Instance?.Config;
            int maxLen = config?.MaxMessageLength ?? 50;
            if (arguments.Count == 0)
            {
                response = $"Usage: {Command} <message> (max {maxLen} chars)";
                return true;
            }
            string msg = string.Join(" ", arguments).Trim();
            if (string.IsNullOrEmpty(msg))
            {
                response = "Message cannot be empty!";
                return false;
            }
            if (msg.Length > maxLen)
            {
                response = $"Message too long! ({msg.Length}/{maxLen})";
                return false;
            }
            PlayerBubbleManager.Instance.ShowTextBubble(player, msg, config?.BubbleDuration ?? 5f);
            response = $"Bubble sent: '{msg}'";
            return true;
        }
    }
} 