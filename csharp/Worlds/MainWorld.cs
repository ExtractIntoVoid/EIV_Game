using ExtractIntoVoid.Managers;
using Godot;
using ModAPI.V2;
using static ExtractIntoVoid.Modding.Multiplayer;
using static ExtractIntoVoid.Modding.WorldEvents;
#if SERVER
using ExtractIntoVoid.Server;
#endif
#if CLIENT
//using ExtractIntoVoid.Client;
#endif


namespace ExtractIntoVoid.Worlds;

public partial class MainWorld : Node
{
    ENetMultiplayerPeer multiplayerPeer = new();
    MultiplayerSpawner Spawner;
    BasicWorld SubWorld;
    public override void _EnterTree()
    {
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
        //StartServer();
        MapManager.LoadMapList();
        var mapname = MapManager.RandomizeMap();
        GD.Print("Map choosen to load: " + mapname);
        if (!GameManager.Instance.SceneManager.TryGetPackedScene(mapname, out var scene))
        {
            GD.PrintErr("Scene cannot be loaded. Quitting...");
            GameManager.Instance.Quit();
        }
        SubWorld = scene.Instantiate<BasicWorld>();
        this.AddChild(SubWorld);
#endif
    }

    public override void _ExitTree()
    {
        Spawner.Spawned -= Spawner_Spawned;
        Spawner.Despawned -= Spawner_Despawned;
    }


#if SERVER
    public void StartServer()
    {
        var Error = multiplayerPeer.CreateServer(NetManager.GetPort(), NetManager.GetMaxClients());
        if (Error != Error.Ok)
        {
            GD.PrintErr($"StartServer error! ({Error}). Quitting...");
            GameManager.Instance.Quit();
        }
        multiplayerPeer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = multiplayerPeer;
        Multiplayer.PeerDisconnected += MultiplayerPeer_PeerDisconnected;
        Multiplayer.PeerConnected += MultiplayerPeer_PeerConnected;
    }
#endif

    #region Calls
    private void Spawner_Despawned(Node node)
    {
        V2EventManager.TriggerEvent(new DeSpawnNode(node));
    }

    private void Spawner_Spawned(Node node)
    {
        V2EventManager.TriggerEvent(new SpawnNode(node));
    }

    private void MultiplayerPeer_PeerDisconnected(long id)
    {
        V2EventManager.TriggerEvent(new OnPeerDisconnect(id));
    }

    private void MultiplayerPeer_PeerConnected(long id)
    {
        V2EventManager.TriggerEvent(new OnPeerConnect(id));
    }
    #endregion
    #region Core related functions.
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
    #endregion
}
