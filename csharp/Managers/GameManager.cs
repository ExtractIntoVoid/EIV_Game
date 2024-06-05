using Godot;
using Godot.Collections;
using System.IO;

namespace ExtractIntoVoid.Managers;

public partial class GameManager : Node
{
    public static GameManager Instance { get; set; }
    public SceneManager SceneManager { get; set; }
    public GodotResourceManager GodotResourceManager { get; set; }
#if MODDABLE
    internal ModManager ModManagerInstance { get; set; }
#endif

#if GAME
    public static string INI = Path.Combine(Path.GetDirectoryName(OS.GetExecutablePath()), "Game.ini");
#elif SERVER
    public static string INI = Path.Combine(Path.GetDirectoryName(OS.GetExecutablePath()), "Server.ini");
#elif CLIENT
    public static string INI = Path.Combine(Path.GetDirectoryName(OS.GetExecutablePath()), "Client.ini");
#endif

#if CLIENT || GAME
    public Client.UIManager UIManager { get; set; }
#endif

    Array<Node> Nodes = new();

    public BuildType BuildType { get =>
#if GAME
            BuildType.Game;
#elif CLIENT
            BuildType.Client;
#elif SERVER
            BuildType.Server;
#else
            BuildType.None;
#endif
    }

    public VersionType VersionType { get =>
#if DEBUG && TESTING
            VersionType.Testing;
#elif DEBUG
            VersionType.Development;
#else
            VersionType.Release; 
#endif
    }

    public override void _EnterTree()
    {
        Instance = this;
        SceneManager = new();
        GodotResourceManager = new();
#if MODDABLE
        ModManagerInstance = new();
        Nodes.Add(ModManagerInstance);
#endif
#if CLIENT || GAME
        UIManager = new();
#endif
        GD.Print(Properties.Resource.BuildDate);
    }

    public override void _Ready()
    {
        foreach (var item in Nodes)
        {
            this.CallDeferred("add_sibling", item);
        }
#if MODDABLE
        ModManagerInstance.AllModsLoaded += ModManagerInstance_AllModsLoaded;
#else
        ModManagerInstance_AllModsLoaded();
#endif
    }

    private void ModManagerInstance_AllModsLoaded()
    {
#if SERVER
        var mw = SceneManager.GetPackedScene("MainWorld").Instantiate();
        this.CallDeferred("add_sibling", mw);
#elif CLIENT || GAME
        var ls = SceneManager.GetPackedScene("LoadingScreen").Instantiate();
        this.CallDeferred("add_sibling", ls);
        UIManager.LoadingScreen = ls as Control;
        var MainMenu = SceneManager.GetPackedScene("MainMenu").Instantiate();
        this.CallDeferred("add_sibling", MainMenu);
#endif
    }

    public void Quit()
    {
        GetTree().Quit();
    }
}
