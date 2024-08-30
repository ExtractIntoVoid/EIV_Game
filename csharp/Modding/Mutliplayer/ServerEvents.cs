using ExtractIntoVoid.Modding.BaseEvents;
using ExtractIntoVoid.Worlds;

namespace ExtractIntoVoid.Modding.Mutliplayer;

public class OnServerPeerConnect : MultiplayerMainWorldEvent
{
    public OnServerPeerConnect(long Id, MainWorld world) : base(Id, world) { }
}
public class OnServerPeerDisconnect : MultiplayerMainWorldEvent
{
    public OnServerPeerDisconnect(long Id, MainWorld world) : base(Id, world) { }
}

public class OnServerStarted : MainWorldEvent
{
    public OnServerStarted(MainWorld world) : base(world) { }
}