using Mirror;
using UnityEngine;

namespace BubbleChat.Networking
{
    public struct BubbleSyncMessage : NetworkMessage
    {
        public ushort PlayerId;
        public string Message;
        public Vector3 Position;
    }
} 