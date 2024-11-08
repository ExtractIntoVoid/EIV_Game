using Godot;
using Godot.Collections;
using EIV_Common.Logger;
using Serilog;
using EIV_JsonLib;

namespace ExtractIntoVoid.Managers;

public partial class GameManager : Node
{
    public ILogger logger;
    public static GameManager Instance { get; set; }
    public SceneManager SceneManager { get; set; }
    public GodotResourceManager GodotResourceManager { get; set; }
    public ModManager ModManagerInstance { get; set; }

#if CLIENT || GAME
    public Client.UIManager UIManager { get; set; }
#endif

    public PlatformManager Platform_Manager { get; set; }

    Array<Node> Nodes = new();

    public override void _Ready()
    {
        MainLog.CreateNew();
        logger = MainLog.logger;
        logger.Information(csharp.Properties.Resource.BuildDate);
        logger.Information(BuildDefined.FullVersion);
        Instance = this;
        logger.Verbose("Preload JsonLib! " + (JsonLibConverters.ModdedConverters.Count == 1));
        SceneManager = new();
        GodotResourceManager = new();
        ModManagerInstance = new();
        Nodes.Add(ModManagerInstance);
#if CLIENT || GAME
        UIManager = new();
#endif
        Platform_Manager = new();
        foreach (var item in Nodes)
        {
            this.CallDeferred("add_sibling", item);
        }
        ModManagerInstance.AllModsLoaded += ModManagerInstance_AllModsLoaded;
        
    }

    private void ModManagerInstance_AllModsLoaded()
    {
        Platform_Manager.Init();
        if (Platform_Manager.Platform.PlatformType == EIV_Common.Platform.PlatformType.Unknown)
        {
            logger.Error("The Platform set to unknown. We dont know what Platform you using. Please use EIV_Platform.Free solution!");
            Quit();
        }

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
        MainLog.Close();
        GetTree().Quit();
    }
}
