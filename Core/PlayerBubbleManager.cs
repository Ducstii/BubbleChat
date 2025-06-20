using BubbleChat.Config;
using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using AdminToys;
using Mirror;
using System.Reflection;
using System.Linq;

namespace BubbleChat.Core
{
    public class PlayerBubbleManager
    {
        public static PlayerBubbleManager Instance { get; } = new PlayerBubbleManager();
        private readonly Dictionary<Player, (GameObject, CoroutineHandle)> _activeBubbles = new();
        private static readonly Dictionary<string, List<string>> _allBlacklistedWords = new Dictionary<string, List<string>>
        {
            { "Racial Slur", new List<string> { "nigger", "nigga", "n1gger", "n1gg3r", "chink", "gook", "spic" } },
            { "Homophobic Slur", new List<string> { "faggot", "fagot", "fggt", "dyke" } },
            { "Transphobic Slur", new List<string> { "tranny", "trannie", "shemale" } },
            { "Ableist Slur", new List<string> { "retard", "r3tard", "spastic" } },
            { "Antisemitic Slur", new List<string> { "kike" } }
        };
        public static Dictionary<string, List<string>> AllBlacklistedWords => _allBlacklistedWords;
        public void Enable() { }
        public void Disable() { foreach (var kv in _activeBubbles) RemoveBubble(kv.Key); _activeBubbles.Clear(); }
        public void ShowTextBubble(Player player, string message, float duration)
        {
            RemoveBubble(player);
            var prefab = GetTextDisplayPrefab();
            if (prefab == null) return;
            var obj = UnityEngine.Object.Instantiate(prefab, player.CameraTransform.position + Vector3.up * 0.4f, Quaternion.identity);
            var toy = obj.GetComponent<AdminToyBase>();
            if (toy != null)
            {
                string formatted = FormatBubbleText(message);
                SetTextToyProperties(toy, formatted, 0.1f);
                NetworkServer.Spawn(obj);
                var handle = Timing.RunCoroutine(FollowHeadCoroutine(player, obj, duration));
                _activeBubbles[player] = (obj, handle);
            }
        }
        public void RemoveBubble(Player player)
        {
            if (_activeBubbles.TryGetValue(player, out var kv) && kv.Item1 != null)
            {
                NetworkServer.UnSpawn(kv.Item1);
                UnityEngine.Object.Destroy(kv.Item1);
                _activeBubbles.Remove(player);
            }
        }
        private GameObject GetTextDisplayPrefab()
        {
            foreach (var prefab in NetworkManager.singleton.spawnPrefabs)
            {
                if (prefab.name == "TextToy")
                    return prefab;
            }
            return null;
        }
        private void SetTextToyProperties(AdminToyBase toy, string text, float size)
        {
            var type = toy.GetType();
            var textFormatField = type.GetField("_textFormat", BindingFlags.NonPublic | BindingFlags.Instance);
            if (textFormatField != null)
                textFormatField.SetValue(toy, text);
            var scaleField = type.GetField("Scale", BindingFlags.Public | BindingFlags.Instance);
            if (scaleField != null)
                scaleField.SetValue(toy, Vector3.one * size);
        }
        private string FormatBubbleText(string message)
        {
            string prefixColor = "4FC3F7"; // Blue
            string messageColor = "81C784"; // Green
            int prefixSize = 3;
            int messageSize = 3;
            return $"<size={prefixSize}><color=#{prefixColor}>Chat:</color></size>\n<size={messageSize}><color=#{messageColor}>{message}</color></size>";
        }
        private IEnumerator<float> FollowHeadCoroutine(Player player, GameObject obj, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration && player != null && player.IsAlive && obj != null)
            {
                obj.transform.position = player.CameraTransform.position + Vector3.up * 0.5f;
                elapsed += 0.02f;
                yield return Timing.WaitForSeconds(0.02f);
            }
            RemoveBubble(player);
        }
        public void RefreshBlacklistFromConfig()
        {
            // Remove any previous custom words
            if (_allBlacklistedWords.ContainsKey("Custom"))
                _allBlacklistedWords.Remove("Custom");
            var config = BubbleChatPlugin.Instance?.Config;
            if (config != null && !string.IsNullOrWhiteSpace(config.CustomBlacklistedWords))
            {
                var customWords = config.CustomBlacklistedWords.Split(',').Select(w => w.Trim()).Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
                if (customWords.Count > 0)
                    _allBlacklistedWords["Custom"] = customWords;
            }
        }
    }
} 