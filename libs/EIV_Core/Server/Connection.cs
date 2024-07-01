#if SERVER
using ExtractIntoVoid;
using ExtractIntoVoid.Extensions;
using ExtractIntoVoid.Managers;
using ExtractIntoVoid.Modding;
using ExtractIntoVoid.Modding.BaseEvents;
using ExtractIntoVoid.Modding.Mutliplayer;
using ExtractIntoVoid.Physics;
using Godot;
using System.IO;

namespace EIV_Core.Server;

public class Connection
{
    static List<long> ClientIds = new();

    public static void PeerConnected(OnServerPeerConnect onServerPeerConnect)
    {
        if (onServerPeerConnect.Disable)
            return;

        var jwt = JWT.Builder.JwtBuilder.Create().AddClaim("test", 00).Encode();
        Console.WriteLine(jwt);
        Console.WriteLine(onServerPeerConnect.Id + " Joined!");
        ClientIds.Add(onServerPeerConnect.Id);

        // set to min to and start spawning
        if (ClientIds.Count >= 1)
        {
            var Transform = onServerPeerConnect.World.SubWorld.SpawnPoints.GetRandom().GlobalTransform;
            onServerPeerConnect.World.Spawner.Spawn(new Godot.Collections.Dictionary<string, Variant>()
            {
                { "id", onServerPeerConnect.Id },
                { "spawn_pos", Transform },
            });
        }
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
        // note: no longer need to clear this.
        //onServerPeerDisconnect.World.GetNode(onServerPeerDisconnect.Id.ToString()).QueueFree();
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