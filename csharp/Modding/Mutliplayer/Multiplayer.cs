using Godot;
using ModAPI.V2;

namespace ExtractIntoVoid.Modding
{
    public class Multiplayer
    {
        public class OnPeerConnect : MultiplayerBaseEvent, IDisableCoreEvent
        {
            public OnPeerConnect(long Id) : base(Id) { }
            public bool Disable { get; set; } = false;
        }
        public class OnPeerDisconnect : MultiplayerBaseEvent, IDisableCoreEvent
        {
            public OnPeerDisconnect(long Id) : base(Id) { }
            public bool Disable { get; set; } = false;
        }
    }
}
