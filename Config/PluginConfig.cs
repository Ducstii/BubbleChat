using Exiled.API.Interfaces;
using System.ComponentModel;

namespace BubbleChat.Config
{
    public class PluginConfig : IConfig
    {
        [Description("Enable the plugin")] public bool IsEnabled { get; set; } = true;
        [Description("Enable debug logs")] public bool Debug { get; set; } = false;
        [Description("Max message length")] public int MaxMessageLength { get; set; } = 50;
        [Description("Chat range in meters")] public float ChatRange { get; set; } = 15f;
        [Description("Bubble visible range")] public float BubbleRange { get; set; } = 8f;
        [Description("Bubble duration (seconds)")] public float BubbleDuration { get; set; } = 5f;
        [Description("Bubble text size")] public float BubbleTextSize { get; set; } = 1.0f;
        [Description("Bubble color (hex)")] public string BubbleColor { get; set; } = "#FFFFFF";
        [Description("Prefix color (hex)")] public string PrefixColor { get; set; } = "#00FF00";
        [Description("Bubble height offset")] public float HeightOffset { get; set; } = 0.9f;
        [Description("Enable bobbing animation")] public bool EnableBobbing { get; set; } = true;
        [Description("Bobbing intensity")] public float BobbingIntensity { get; set; } = 0.005f;
    }
} 