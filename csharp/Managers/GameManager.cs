using Godot;
using Godot.Collections;
using System.IO;
using EIV_Common;
using EIV_Common.Logger;
using Serilog;
using EIV_JsonLib;
using System;

namespace ExtractIntoVoid.Managers;

public partial class GameManager : Node
{
    public ILogger logger;
    public static GameManager Instance { get; set; }
    public SceneManager SceneManager { get; set; }
    public GodotResourceManager GodotResourceManager { get; set; }
#if MODDABLE
    public ModManager ModManagerInstance { get; set; }
#endif

#if CLIENT || GAME
    public Client.UIManager UIManager { get; set; }
#endif

    public DRMManager DRM_Manager { get; set; }



    Array<Node> Nodes = new();

    public override void _EnterTree()
    {
        logger.Information(Properties.Resource.BuildDate);
        logger.Information(BuildDefined.FullVersion);
        Instance = this;
        GD.Print("Preload JsonLib! " + (JsonLibConverters.ModdedConverters.Count == 1));
        SceneManager = new();
        GodotResourceManager = new();
#if MODDABLE
        ModManagerInstance = new();
        Nodes.Add(ModManagerInstance);
#endif
#if CLIENT || GAME
        UIManager = new();
#endif
        DRM_Manager = new();
    }

    public override void _Ready()
    {
        MainLog.CreateNew();
        logger = MainLog.logger;
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
        DRM_Manager.Init();
        if (DRM_Manager.DRM.DRMType == EIV_Common.DRM.DRMType.Unknown)
        {
            logger.Error("The DRM set to unknown. We dont know what DRM you using. Please use EIV_DRM.Free solution!");
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
