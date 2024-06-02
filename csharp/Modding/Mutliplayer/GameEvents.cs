
using ExtractIntoVoid.Modding.BaseEvents;
using ExtractIntoVoid.Worlds;

namespace ExtractIntoVoid.Modding.Mutliplayer;

public class OnGamePeerConnect : MultiplayerMainWorldEvent
{
    public OnGamePeerConnect(long Id, MainWorld world) : base(Id, world) { }
}
public class OnGamePeerDisconnect : MultiplayerMainWorldEvent
{
    public OnGamePeerDisconnect(long Id, MainWorld world) : base(Id, world) { }
}