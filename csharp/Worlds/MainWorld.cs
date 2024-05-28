using Godot;
using Godot.Collections;
using ModAPI.V2;
using static ExtractIntoVoid.Modding.WorldEvents;

namespace ExtractIntoVoid.Worlds;

public partial class MainWorld : Node
{
    ENetMultiplayerPeer multiplayerPeer = new();
    int Port = 9999;
    string Address = "127.0.0.1";
    bool IsHost = false;
    Array<Node> SubWorld = new();
    MultiplayerSpawner Spawner;

    public override void _EnterTree()
    {
        GD.Print("_EnterTree : IP: " + Address + " Port: " + Port);
        multiplayerPeer = new();
        Spawner = GetNode<MultiplayerSpawner>("Spawner");
        Spawner.Spawned += Spawner_Spawned;
        Spawner.Despawned += Spawner_Despawned;
        OnStartWorld onStartWorld = new()
        {
            SpawnableNodes = new()
            {
                "res://scenes/Character/Player.tscn"
            }
        };

        V2EventManager.TriggerEvent(onStartWorld);

        // Make sure if we remove all we can spawn as default player.
        if (onStartWorld.SpawnableNodes.Count != 0)
            Spawner.AddSpawnableScene("res://scenes/Character/Player.tscn");
        else
            //  Otherwise we add into spawnable scene.
            onStartWorld.SpawnableNodes.ForEach(Spawner.AddSpawnableScene);
    }

    public override void _Ready()
    {
#if SERVER
        //We starting server here already.

#endif
    }

    private void Spawner_Despawned(Node node)
    {
        DeSpawnNode despawnNode = new(node);
        V2EventManager.TriggerEvent(despawnNode);
    }

    private void Spawner_Spawned(Node node)
    {
        SpawnNode spawnNode = new(node);
        V2EventManager.TriggerEvent(spawnNode);
    }

    public void Core_DeSpawnNode(DeSpawnNode node)
    {
        if (node.Disable)
            return;
        // do stuff
    }

    public void Core_SpawnNode(SpawnNode node)
    {
        if (node.Disable)
            return;
        // do stuff
    }
}
