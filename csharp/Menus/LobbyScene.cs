#if CLIENT || GAME
using Godot;

namespace ExtractIntoVoid.Menus;

public partial class LobbyScene : Control
{
    [Export]
    public Label ServerText;
    public string LobbyAddress;

    public string UserToken;

    public System.Net.Http.HttpClient Client;

    public override void _Ready()
    {
        Client = new();
    }

    public void Connect()
    {
        var connectasync = Client.GetAsync($"http://{LobbyAddress}/EIV_Lobby/Connect");
        connectasync.Wait(100);
        // we could not connect. Just quit.
        if (!connectasync.IsCompletedSuccessfully)
            Quit();

        UserToken = connectasync.Result.Content.ReadAsStringAsync().Result;
    }


    public void StartPlay()
    {
        // Use the Socket. Sadly we now have to!
        // All those should be inside CLIENT!

        /*
        // Map Join
        var mainWorld = SceneManager.GetPackedScene("MainWorld").Instantiate<MainWorld>();
        this.CallDeferred("add_sibling", mainWorld);
        mainWorld.CallDeferred("StartClient", ServerAddress, ServerPort, ServerMap);
        */
    }

    public void Quit()
    {
        Client.Dispose();
        if (GetParent() is ConnectionScreen connectionScreen)
        {
            connectionScreen.Show();
        }
        this.QueueFree();
    }

}
#endif