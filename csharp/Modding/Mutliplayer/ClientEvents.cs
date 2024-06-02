using ExtractIntoVoid.Modding.BaseEvents;
using ExtractIntoVoid.Worlds;

namespace ExtractIntoVoid.Modding.Mutliplayer;

public class OnClientPeerConnect : MultiplayerMainWorldEvent
{
    public OnClientPeerConnect(long Id, MainWorld world) : base(Id, world) { }
}
public class OnClientPeerDisconnect : MultiplayerMainWorldEvent
{
    public OnClientPeerDisconnect(long Id, MainWorld world) : base(Id, world) { }
}

public class OnServerDisconnected : MainWorldEvent
{
    public OnServerDisconnected(MainWorld world) : base(world) { }
}

public class OnConnectionFailed : MainWorldEvent
{
    public OnConnectionFailed(MainWorld world) : base(world) { }
}

public class OnConnectedToServer : MainWorldEvent
{
    public OnConnectedToServer(MainWorld world) : base(world) { }
}