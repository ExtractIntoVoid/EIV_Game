﻿using ExtractIntoVoid.Managers;
using Godot;
using ModAPI.V2;
using static ExtractIntoVoid.Modding.WorldEvents;
using ExtractIntoVoid.Modding.Mutliplayer;

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
        StartServer();
        MapManager.LoadMapList();
        var mapname = MapManager.RandomizeMap();
        GD.Print("Map choosen to load: " + mapname);
        if (!GameManager.Instance.SceneManager.TryGetPackedScene(mapname, out var scene))
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
        Spawner.Spawned -= Spawner_Spawned;
        Spawner.Despawned -= Spawner_Despawned;
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
        var Error = multiplayerPeer.CreateServer(NetManager.GetPort(), NetManager.GetMaxClients());
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
        if (!GameManager.Instance.SceneManager.TryGetPackedScene(SubMapName, out var scene))
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
        V2EventManager.TriggerEvent(new OnServerDisconnected(this));
    }

    private void Multiplayer_ConnectionFailed()
    {
        V2EventManager.TriggerEvent(new OnConnectionFailed(this));
    }

    private void Multiplayer_ConnectedToServer()
    {
        V2EventManager.TriggerEvent(new OnConnectedToServer(this));
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
#if GAME
        V2EventManager.TriggerEvent(new OnGamePeerDisconnect(id, this));
#elif SERVER
        V2EventManager.TriggerEvent(new OnServerPeerDisconnect(id, this));
#elif CLIENT
        V2EventManager.TriggerEvent(new OnClientPeerDisconnect(id, this));
#endif
    }

    private void MultiplayerPeer_PeerConnected(long id)
    {
#if GAME
        V2EventManager.TriggerEvent(new OnGamePeerConnect(id, this));
#elif SERVER
        V2EventManager.TriggerEvent(new OnServerPeerConnect(id, this));
#elif CLIENT
        V2EventManager.TriggerEvent(new OnClientPeerConnect(id, this));
#endif
    }
    #endregion

    public static void test(OnClientPeerConnect onClientPeerConnect)
    {

    }
}