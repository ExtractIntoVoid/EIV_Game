using ExtractIntoVoid.Managers;
using ExtractIntoVoid.Worlds;
using Godot;

namespace ExtractIntoVoid.Menus;

public partial class LobbyScene : Control
{
    [Export]
    public Label ServerText;

    public string ServerAddress;
    public int ServerPort;
    public string ServerMap;

    public void StartPlay()
    {
        var mainWorld = SceneManager.GetPackedScene("MainWorld").Instantiate<MainWorld>();
        this.CallDeferred("add_sibling", mainWorld);
        mainWorld.CallDeferred("StartClient", ServerAddress, ServerPort, ServerMap);
    }

    public void Quit()
    {
        if (GetParent() is ConnectionScreen connectionScreen)
        {
            connectionScreen.Show();
        }
        this.QueueFree();
    }

}
