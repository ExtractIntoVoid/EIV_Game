using ExtractIntoVoid.Managers;
using Godot;
using ModAPI.V2;
using static ExtractIntoVoid.Modding.WorldEvents;
using ExtractIntoVoid.Modding.Mutliplayer;
using ExtractIntoVoid.Modding;

#if GAME
using ExtractIntoVoid.Server;
using ExtractIntoVoid.Client;
#elif SERVER
using ExtractIntoVoid.Server;
#elif CLIENT
using ExtractIntoVoid.Client;
#endif


namespace ExtractIntoVoid.Worlds;

public partial class MainWorld : Node
{
    ENetMultiplayerPeer multiplayerPeer = new();
    public MultiplayerSpawner Spawner;
    public BasicWorld SubWorld;

    public object V2EventManager { get; private set; }

    public override void _EnterTree()
    {
        multiplayerPeer = new();
        Spawner = GetNode<MultiplayerSpawner>("Spawner");
        Spawner.SpawnFunction = new Callable(this, "Spawn_Node");
        OnStartWorld onStartWorld = new()
        {
            SpawnableNodes = new()
            {
                "res://scenes/Character/Player.tscn"    // 0 or first should always be the PLAYER!
            }
        };

        V2Manager.TriggerEvent(onStartWorld);

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
        StartServer();
        MapManager.LoadMapList();
        var mapname = MapManager.RandomizeMap();
        if (ArgManager.HasArgParam("map"))
            mapname = ArgManager.GetArgParam("map");      
        GD.Print("Map choosen to load: " + mapname);
        if (!SceneManager.TryGetPackedScene(mapname, out var scene))
        {
            GD.PrintErr("Scene cannot be loaded. Quitting...");
            GameManager.Instance.Quit();
            return;
        }
        SubWorld = scene.Instantiate<BasicWorld>();
        this.AddChild(SubWorld);
#endif
    }

    public override void _ExitTree()
    {
        if (Multiplayer.MultiplayerPeer == multiplayerPeer)
        {
            Multiplayer.PeerDisconnected -= MultiplayerPeer_PeerDisconnected;
            Multiplayer.PeerConnected -= MultiplayerPeer_PeerConnected;
#if CLIENT || GAME
            Multiplayer.ConnectedToServer -= Multiplayer_ConnectedToServer;
            Multiplayer.ConnectionFailed -= Multiplayer_ConnectionFailed;
            Multiplayer.ServerDisconnected -= Multiplayer_ServerDisconnected;
#endif
            Multiplayer.MultiplayerPeer.Close();
        }
            
    }


#if SERVER|| GAME
    public void StartServer()
    {
        var Error = multiplayerPeer.CreateServer(NetManager.GetPort());
        if (Error != Error.Ok)
        {
            GD.PrintErr($"StartServer error! ({Error}). Quitting...");
            GameManager.Instance.Quit();
            return;
        }
        multiplayerPeer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = multiplayerPeer;
        Multiplayer.PeerDisconnected += MultiplayerPeer_PeerDisconnected;
        Multiplayer.PeerConnected += MultiplayerPeer_PeerConnected;
        V2Manager.TriggerEvent(new OnServerStarted(this));
    }
#endif
#if CLIENT || GAME
    public void StartClient(string Address, int port, string SubMapName) 
    {
        var Error = multiplayerPeer.CreateClient(Address, port);
        if (Error != Error.Ok)
        {
            GD.PrintErr($"StartClient error! ({Error}). Quitting...");
            GameManager.Instance.Quit();
            return;
        }
        multiplayerPeer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = multiplayerPeer;
        Multiplayer.PeerDisconnected += MultiplayerPeer_PeerDisconnected;
        Multiplayer.PeerConnected += MultiplayerPeer_PeerConnected;
        Multiplayer.ConnectedToServer += Multiplayer_ConnectedToServer;
        Multiplayer.ConnectionFailed += Multiplayer_ConnectionFailed;
        Multiplayer.ServerDisconnected += Multiplayer_ServerDisconnected;
        GD.Print("Map choosen to load: " + SubMapName);
        if (!SceneManager.TryGetPackedScene(SubMapName, out var scene))
        {
            GD.PrintErr("Scene cannot be loaded. Quitting...");
            GameManager.Instance.Quit();
            return;
        }
        SubWorld = scene.Instantiate<BasicWorld>();
        this.AddChild(SubWorld);
    }

    private void Multiplayer_ServerDisconnected()
    {
        V2Manager.TriggerEvent(new OnServerDisconnected(this));
    }

    private void Multiplayer_ConnectionFailed()
    {
        V2Manager.TriggerEvent(new OnConnectionFailed(this));
    }

    private void Multiplayer_ConnectedToServer()
    {
        V2Manager.TriggerEvent(new OnConnectedToServer(this));
    }
#endif

    #region Calls

    private void MultiplayerPeer_PeerDisconnected(long id)
    {
#if GAME
        V2Manager.TriggerEvent(new OnGamePeerDisconnect(id, this));
#elif SERVER
        V2Manager.TriggerEvent(new OnServerPeerDisconnect(id, this));
#elif CLIENT
        V2Manager.TriggerEvent(new OnClientPeerDisconnect(id, this));
#endif
    }

    private void MultiplayerPeer_PeerConnected(long id)
    {
#if GAME
        V2Manager.TriggerEvent(new OnGamePeerConnect(id, this));
#elif SERVER
        V2Manager.TriggerEvent(new OnServerPeerConnect(id, this));
#elif CLIENT
        V2Manager.TriggerEvent(new OnClientPeerConnect(id, this));
#endif
    }
    #endregion

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferChannel = 10)]
    public void CustomRPC(string eventToCall, Godot.Collections.Array<Variant> variants)
    {
        RPC_EventManager.CallMethod(eventToCall, variants);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferChannel = 10)]
    public void CustomRPC_NoLocal(string eventToCall, Godot.Collections.Array<Variant> variants)
    {
        RPC_EventManager.CallMethod(eventToCall, variants);
    }

    public Node Spawn_Node(Variant variant)
    {
        OnSpawnNode onSpawnNode = new(variant, this);
        V2Manager.TriggerEvent(onSpawnNode);
        return onSpawnNode.ReturnerNode;
    }
}
