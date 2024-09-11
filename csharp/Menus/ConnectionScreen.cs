#if CLIENT || GAME
using EIV_Common;
using EIV_Common.InfoJSON;
using ExtractIntoVoid.Managers;
using ExtractIntoVoid.Worlds;
using Godot;
using Newtonsoft.Json;
using System.IO;

namespace ExtractIntoVoid.Menus;

public partial class ConnectionScreen : Control
{
	VBoxContainer ServerList;
    InputScene InputScene;
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
        var lobbyList = ConfigINI.Read(BuildDefined.INI, "Lobby", "OfflineLobbyList");
        if (lobbyList.Contains("{EXE}"))
        {
            lobbyList.Replace("{EXE}", Directory.GetCurrentDirectory());
        }
        if (lobbyList.Contains("{GAMEDATA}"))
        {
            lobbyList.Replace("{GAMEDATA}", "user://");
            lobbyList = ProjectSettings.GlobalizePath(lobbyList);
        }
        var ipList = File.ReadAllLines(lobbyList);
        foreach (var item in ipList)
        {
            CallbackButton(item);
        }
        GameManager.Instance.UIManager.LoadScreenStop();
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



    public void AddServerManually()
    {
        if (!GameManager.Instance.SceneManager.TryGetPackedScene("InputScene", out var scene))
            return; // Display error ?
        InputScene = scene.Instantiate<InputScene>();
        InputScene.Question.Text = "Please enter lobby IP below with ports!";
        InputScene.ShowInputOnly();
        InputScene.ButtonCallback(null, null, CallbackButton);
        this.AddChild(InputScene);
    }

    public void CallbackButton(string answer)
    {
        if (InputScene != null)
        {
            InputScene.QueueFree();
            InputScene = null;
        }
        if (string.IsNullOrEmpty(answer))
            return;
        // GET to lobby.
        System.Net.Http.HttpClient httpClient = new();
        var rsp = httpClient.GetAsync(answer + "/EIV_Lobby/About").Result;
        if (rsp.StatusCode != System.Net.HttpStatusCode.OK)
            return;
        var result = rsp.Content.ReadAsStringAsync().Result;
        var serverInfo = JsonConvert.DeserializeObject<ServerInfoJSON>(result);
        if (serverInfo == null) 
            return;
        ServerData serverData = new()
        { 
            Address = serverInfo.Connection.ServerAddress,
            Port = serverInfo.Connection.ServerPort,
            MapName = serverInfo.Game.Map,
            MaxPlayerCount = serverInfo.Game.MaxPlayerNumbers,
            PlayerCount = serverInfo.Game.PlayerNumbers,
            ServerInfo = serverInfo.Server.ServerDescription,
            ServerName = serverInfo.Server.ServerName,
        };
        CreateServer(serverData);
    }
}
#endif