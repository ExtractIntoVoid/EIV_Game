#if SERVER
using ExtractIntoVoid;
using ExtractIntoVoid.Modding;
using ExtractIntoVoid.Modding.BaseEvents;
using ExtractIntoVoid.Modding.Mutliplayer;
using Godot;

namespace EIV_Core.Server;

public class Connection
{
    static List<long> ClientIds = new();

    public static void PeerConnected(OnServerPeerConnect onServerPeerConnect)
    {
        if (onServerPeerConnect.Disable)
            return;

        Console.WriteLine(onServerPeerConnect.Id + " Joined!");
        ClientIds.Add(onServerPeerConnect.Id);

        // set to min to and start spawning
        if (ClientIds.Count >= 1)
        {
            onServerPeerConnect.World.Spawner.Spawn(new Godot.Collections.Dictionary<string, string>()
            {
                { "id", onServerPeerConnect.Id.ToString() }
            });
        }

        Godot.Collections.Dictionary<StringName, PackedScene> someDictionary = new()
        {
           { "FunnyName", ResourceLoader.Load<PackedScene>("res://yeet.tscn") }
        };
        /*
        onServerPeerConnect.World.Rpc("CustomRPC", "RPC_TEST_SERVER", new Godot.Collections.Array<Variant>() { onServerPeerConnect.Id, true, true, false });
        onServerPeerConnect.World.Rpc("CustomRPC", "RPC_TEST_CLIENT", new Godot.Collections.Array<Variant>() { onServerPeerConnect.Id, true, false, false });
        onServerPeerConnect.World.Rpc("CustomRPC_NoLocal", "RPC_TEST_SERVER", new Godot.Collections.Array<Variant>() { onServerPeerConnect.Id, false, true, false });
        onServerPeerConnect.World.Rpc("CustomRPC_NoLocal", "RPC_TEST_CLIENT", new Godot.Collections.Array<Variant>() { onServerPeerConnect.Id, false, false, false });
        */
    }





    [CustomRPC(callOnBuild: BuildType.Server)]
    public static void RPC_TEST_SERVER(List<Variant> variants)
    {
        int i = ((int)variants[0]);
        bool IsLocal = ((bool)variants[1]);
        bool ToServerCall = ((bool)variants[2]);
        bool FromClient = ((bool)variants[3]);
        Console.WriteLine("RPC_TEST_SERVER, i: " + i + " Is Local? " + IsLocal + " ToServer call? " + ToServerCall + " From Client? " + FromClient);
    }

    public static void PeerDisconnected(OnServerPeerDisconnect onServerPeerDisconnect)
    {
        if (onServerPeerDisconnect.Disable)
            return;

        Console.WriteLine(onServerPeerDisconnect.Id + " Left!");
        ClientIds.Remove(onServerPeerDisconnect.Id);
    }
}
#endif