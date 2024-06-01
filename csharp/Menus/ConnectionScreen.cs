using ExtractIntoVoid.Managers;
using ExtractIntoVoid.Worlds;
using Godot;

namespace ExtractIntoVoid.Menus;

//  Todo: strip out this and others from Server Build, server doesnt need this data.
public partial class ConnectionScreen : Control
{
	VBoxContainer ServerList;
    public struct ServerData 
    {
        public string Address;
        public int Port;
        public string ServerName;
        public string ServerInfo;
        public int PlayerCount;
        public int MaxPlayerCount;
        public string MapName;
    }
    public override void _Ready()
	{
        ServerList = GetNode<VBoxContainer>("%ServerList");
        CreateServer(new()
        { 
            Address = "192.168.1.50",
            Port = 8888,
            ServerName = "SlejmUr's Official Server - Vanilla",
            ServerInfo = "Vanilla, No modification.",
            MapName = "TestWorld",
            PlayerCount = 0,
            MaxPlayerCount = 100,
        });
        CreateServer(new()
        {
            Address = "192.168.1.50",
            Port = 8889,
            ServerName = "SlejmUr's Official Server - Modded",
            ServerInfo = "Modded. Contains 1.5x big every weapon. Initial Gear P90 and AWP.\nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
            MapName = "TestWorld",
            PlayerCount = 66,
            MaxPlayerCount = 100,
        });
        CreateServer(new()
        {
            Address = "192.168.1.30",
            Port = 3223,
            ServerName = "This should be hidden!",
            ServerInfo = "AAAAAAAAAAAAAAAAAAAAAAAA",
            MapName = "YEETWORLD",
            PlayerCount = 13,
            MaxPlayerCount = 2,
        });
    }

	public void CreateServer(ServerData data)
	{
        // Skip map names
        if (!GameManager.Instance.SceneManager.Scenes.ContainsKey(data.MapName))
            return;

		Label serverName = new()
		{ 
            Text = data.ServerName,
			LabelSettings = new()
            {
                FontSize = 30
            }
        };
        Label serverInfo = new()
        {
            Position = new(0, 40),
            Text = data.ServerInfo
        };
        Label ServerPlayerCount = new()
        {
            Position = new(600, 10),
            Text = $"{data.PlayerCount}/{data.MaxPlayerCount}"
        };
        Button connectButton = new()
        {
            Text = "Connect"
        };
        connectButton.Pressed += () => 
        {
#if CLIENT
            var mainWorld = GameManager.Instance.SceneManager.GetPackedScene("MainWorld").Instantiate<MainWorld>();
            this.CallDeferred("add_sibling", mainWorld);
            mainWorld.StartClient(data.Address, data.Port, data.MapName);
#endif
        };
        Control control = new();
        control.AddChild(serverName);
        control.AddChild(serverInfo);
        control.AddChild(ServerPlayerCount);
        ServerList.AddChild(control);
    }
}
