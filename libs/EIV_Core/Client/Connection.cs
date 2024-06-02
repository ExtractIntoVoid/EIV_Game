#if CLIENT
using ExtractIntoVoid.Modding.Mutliplayer;

namespace EIV_Core.Client;

public class Connection
{
    public static void PeerConnected(OnClientPeerConnect onClientPeerConnect)
    {
        if (onClientPeerConnect.Disable)
            return;

        Console.WriteLine(onClientPeerConnect.Id + " Joined!");
    }

    public static void PeerDisconnected(OnClientPeerDisconnect onClientPeerDisconnect)
    {
        if (onClientPeerDisconnect.Disable)
            return;

        Console.WriteLine(onClientPeerDisconnect.Id + " Left!");
    }
}
#endif