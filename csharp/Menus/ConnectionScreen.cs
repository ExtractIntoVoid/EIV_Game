#if CLIENT || GAME
using EIV_Common;
using EIV_JsonLib;
using ExtractIntoVoid.Managers;
using Godot;
using Serilog;
using System.IO;
using System.Text.Json;

namespace ExtractIntoVoid.Menus;

public partial class ConnectionScreen : Control
{
	VBoxContainer ServerList;
    InputScene InputScene;
    public override void _Ready()
	{
        ServerList = GetNode<VBoxContainer>("%ServerList");
        Refresh();
    }

	public void CreateServer(ServerInfoJson serverInfojson, string address)
	{
		Label serverName = new()
		{ 
            Text = serverInfojson.LobbyInfo.Name,
			LabelSettings = new()
            {
                FontSize = 30
            }
        };
        Label serverInfo = new()
        {
            Position = new(0, 40),
            Text = serverInfojson.LobbyInfo.Description
        };
        Label ServerPlayerCount = new()
        {
            Position = new(600, 10),
            Text = $"{serverInfojson.LobbyInfo.PlayerNumbers}/{serverInfojson.LobbyInfo.MaxPlayerNumbers}",
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
            var lobby = SceneManager.GetPackedScene("LobbyScene").Instantiate<LobbyScene>();
            this.CallDeferred("add_sibling", lobby);
            lobby.LobbyAddress = address;
            lobby.ServerText.Text = serverInfojson.LobbyInfo.Name;
            lobby.Connect();
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
        if (!SceneManager.TryGetPackedScene("InputScene", out var scene))
            return; // Display error ?
        InputScene = scene.Instantiate<InputScene>();
        this.GetParent().AddChild(InputScene);
        this.Hide();
        InputScene.Question.Text = "Please enter lobby IP and ports below!";
        InputScene.ShowInputOnly();
        InputScene.ButtonCallback(null, null, CallbackButton);
    }

    public void CallbackButton(string answer)
    {
        this.Show();
        if (InputScene != null)
        {
            InputScene.QueueFree();
            InputScene = null;
        }
        if (string.IsNullOrEmpty(answer))
            return;
        // GET to lobby.
        System.Net.Http.HttpClient httpClient = new();
        var rsp = httpClient.GetAsync($"http://{answer}/EIV_Lobby/About");
        rsp.Wait(100);
        if (rsp.IsCompletedSuccessfully)
        {
            if (rsp.Result.StatusCode != System.Net.HttpStatusCode.OK)
                return;
            var result = rsp.Result.Content.ReadAsStringAsync().Result;
            var serverInfo = JsonSerializer.Deserialize<ServerInfoJson>(result);
            if (serverInfo == null)
                return;
            CreateServer(serverInfo, answer);
        }
        else
        {
            Log.Warning($"Result for {answer} is Failed!");
        }
    }

    public void Refresh()
    {
        GameManager.Instance.UIManager.LoadScreenStart();
        foreach (var item in ServerList.GetChildren())
        {
            ServerList.RemoveChild(item);
        }
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
        if (!File.Exists(lobbyList))
        {
            GameManager.Instance.UIManager.LoadScreenStop();
            return;
        }
        var ipList = File.ReadAllLines(lobbyList);
        foreach (var item in ipList)
        {
            CallbackButton(item);
        }
        GameManager.Instance.UIManager.LoadScreenStop();

    }
}
#endif