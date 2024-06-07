#if CLIENT
using ExtractIntoVoid.Modding.Mutliplayer;
using ExtractIntoVoid.Modding.BaseEvents;
using ExtractIntoVoid.Modding;
using Godot;
using ExtractIntoVoid;
using ExtractIntoVoid.Physics;

namespace EIV_Core.Client;

public class Connection
{
    public static void PeerConnected(OnClientPeerConnect onClientPeerConnect)
    {
        if (onClientPeerConnect.Disable)
            return;

        Console.WriteLine(onClientPeerConnect.Id + " Joined!");
        /*
        onClientPeerConnect.World.Rpc("CustomRPC", "RPC_TEST_SERVER", new Godot.Collections.Array<Variant>() { onClientPeerConnect.Id, true , true, true });
        onClientPeerConnect.World.Rpc("CustomRPC", "RPC_TEST_CLIENT", new Godot.Collections.Array<Variant>() { onClientPeerConnect.Id, true, false, true });
        onClientPeerConnect.World.Rpc("CustomRPC_NoLocal", "RPC_TEST_SERVER", new Godot.Collections.Array<Variant>() { onClientPeerConnect.Id, false, true, true });
        onClientPeerConnect.World.Rpc("CustomRPC_NoLocal", "RPC_TEST_CLIENT", new Godot.Collections.Array<Variant>() { onClientPeerConnect.Id, false, false, true });
        */
    }


    [CustomRPC(callOnBuild: BuildType.Client)]
    public static void RPC_TEST_CLIENT(List<Variant> variants)
    {
        int i = ((int)variants[0]);
        bool IsLocal = ((bool)variants[1]);
        bool ToServerCall = ((bool)variants[2]);
        bool FromClient = ((bool)variants[3]);
        Console.WriteLine("RPC_TEST_CLIENT, i: " + i  + " Is Local? " + IsLocal + " ToServer call? " + ToServerCall + " From Client? " + FromClient);
    }

    public static void PeerDisconnected(OnClientPeerDisconnect onClientPeerDisconnect)
    {
        if (onClientPeerDisconnect.Disable)
            return;

        Console.WriteLine(onClientPeerDisconnect.Id + " Left!");
    }

    public static void OnSpawnNode(WorldEvents.OnSpawnNode onSpawnNode)
    {
        if (onSpawnNode.Disable)
            return;
        Console.WriteLine(onSpawnNode.InputVariant + " OnSpawn!");

        var Dict = (Godot.Collections.Dictionary<string, Variant>)onSpawnNode.InputVariant;
        var scene = onSpawnNode.World.Spawner.GetSpawnableScene(0);

        if (!ResourceLoader.Exists(scene))
        {
            GD.PrintErr("Scene is not exist!");
            return;
        }
        var myNode = ResourceLoader.Load<PackedScene>(scene).Instantiate();
        myNode.Name = (Dict["id"].AsInt32()).ToString();
        var player = myNode as Player;
        if (player == null)
        {
            GD.PrintErr("myNode should be the player but not!");
            // myNode should be the player but not. That is not good so we just return.
            return;
        }
        player.GlobalTransform = Dict["spawn_pos"].AsTransform3D();
        onSpawnNode.ReturnerNode = player;
        GD.Print("Should spawn");
    }
}
#endif