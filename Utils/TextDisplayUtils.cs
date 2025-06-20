namespace BubbleChat.Utils
{
    public static class TextDisplayUtils
    {
        public static string FormatBubble(string message, string color, string prefixColor)
        {
            return $"<color={prefixColor}><size=1>CHAT:</size></color> <color={color}><size=1>{message}</size></color>";
        }
    }
} 