#if SERVER
using ExtractIntoVoid.Modding.Mutliplayer;

namespace EIV_Core.Server;

public class Connection
{
    public void PeerConnected(OnServerPeerConnect onServerPeerConnect)
    {
        if (onServerPeerConnect.Disable)
            return;

        Console.WriteLine(onServerPeerConnect.Id + " Joined!");
    }

    public void PeerDisconnected(OnServerPeerDisconnect onServerPeerDisconnect)
    {
        if (onServerPeerDisconnect.Disable)
            return;

        Console.WriteLine(onServerPeerDisconnect.Id + " Left!");
    }
}
#endif