using ModAPI.V2;

namespace ExtractIntoVoid.Modding
{
    public class Multiplayer
    {
#if CLIENT
        public class OnClientPeerConnect : MultiplayerBaseEvent, IDisableCoreEvent
        {
            public OnClientPeerConnect(long Id) : base(Id) { }
            public bool Disable { get; set; } = false;
        }
        public class OnClientPeerDisconnect : MultiplayerBaseEvent, IDisableCoreEvent
        {
            public OnClientPeerDisconnect(long Id) : base(Id) { }
            public bool Disable { get; set; } = false;
        }

        public class OnServerDisconnected : BaseEvent, IDisableCoreEvent
        {
            public bool Disable { get; set; } = false;
        }

        public class OnConnectionFailed : BaseEvent, IDisableCoreEvent
        {
            public bool Disable { get; set; } = false;
        }

        public class OnConnectedToServer : BaseEvent, IDisableCoreEvent
        {
            public bool Disable { get; set; } = false;
        }
#endif
#if SERVER
        public class OnServerPeerConnect : MultiplayerBaseEvent, IDisableCoreEvent
        {
            public OnServerPeerConnect(long Id) : base(Id) { }
            public bool Disable { get; set; } = false;
        }
        public class OnServerPeerDisconnect : MultiplayerBaseEvent, IDisableCoreEvent
        {
            public OnServerPeerDisconnect(long Id) : base(Id) { }
            public bool Disable { get; set; } = false;
        }
    }
#endif
}
