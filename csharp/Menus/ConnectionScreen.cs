#if CLIENT || GAME
using ExtractIntoVoid.Managers;
using ExtractIntoVoid.Worlds;
using Godot;

namespace ExtractIntoVoid.Menus;

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
        GameManager.Instance.UIManager.LoadScreenStop();
        ServerList = GetNode<VBoxContainer>("%ServerList");
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
        CreateServer(new()
        { 
            Address = "192.168.1.50",
            Port = 7878,
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

        for (int i = 0; i < 5; i++)
            CreateServer(new()
            {
                Address = "192.168.1.50",
                Port = 8889,
                ServerName = "make it overflow to scroll!",
                ServerInfo = "Modded. Contains 1.5x big every weapon. Initial Gear P90 and AWP.\nAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                MapName = "TestWorld",
                PlayerCount = 66,
                MaxPlayerCount = 100,
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
            Text = $"{data.PlayerCount}/{data.MaxPlayerCount}",
            HorizontalAlignment = HorizontalAlignment.Right,
        };
        Button connectButton = new()
        {
            Text = "Connect",
            Position = new(705, 5),
        };
        connectButton.Pressed += () => 
        {
            this.Hide();
            var mainWorld = GameManager.Instance.SceneManager.GetPackedScene("MainWorld").Instantiate<MainWorld>();
            this.CallDeferred("add_sibling", mainWorld);
            mainWorld.CallDeferred("StartClient", data.Address, data.Port, data.MapName);
        };
        Control control = new();
        control.AddChild(serverName);
        control.AddChild(serverInfo);
        control.AddChild(ServerPlayerCount);
        control.AddChild(connectButton);
        ServerList.AddChild(control);
    }
}
#endif