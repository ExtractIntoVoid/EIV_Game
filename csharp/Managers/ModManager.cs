using ExtractIntoVoid.Data;
using ExtractIntoVoid.Modding;
using Godot;
using ModAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExtractIntoVoid.Managers;

public partial class ModManager : Node
{
    public Dictionary<string, ModData> Mods;

    public event Action AllModsLoaded;

    public static readonly IReadOnlyList<string> ModLocations =
    [
        Path.Combine(Path.GetDirectoryName(OS.GetExecutablePath()), ProjectSettings.GlobalizePath("res://mods")),
        ProjectSettings.GlobalizePath("user://mods"),
    ];

    public override void _Ready()
    {
        Mods = new();
        this.CallDeferred(MethodName.LoadAllMods);
    }

    public void LoadAllMods()
    {
        EIV_Common.ModManager.Init();
        Debugger.ParseLogger(GameManager.Instance.logger);
        foreach (var item in ModLocations)
        {
            GD.Print(item);
            if (!Directory.Exists(item))
                continue;
            string[] modDirectories = Directory.GetDirectories(item);
            foreach (var mod in modDirectories)
            {
                if (mod.Contains("Dependencies"))
                    continue;
                LoadMod(mod);
            }
        }
        RPC_EventManager.Load();
        AllModsLoaded?.Invoke();
    }

    void LoadMod(string modDir)
    {
        if (!File.Exists(Path.Combine(modDir, "Mod.json")))
        {
            GD.Print($"Mod.json not found skipping {modDir} dir");
            return;
        }

        var modjson = JsonConvert.DeserializeObject<ModJson>(File.ReadAllText(Path.Combine(modDir, "Mod.json")));
        ModData modData = new()
        {
            AssemblyName = modjson.InternalName + ".dll",
            ResourcePack = modjson.InternalName + ".pck",
            ModJson = modjson,
#if SERVER
            SaveDir = Path.Combine(modDir, "ModSave"),
#endif
            Hashes = new()
        };
        MainLoader.LoadMod(Path.Combine(modDir, modData.AssemblyName));
        ProjectSettings.LoadResourcePack(Path.Combine(modDir, modData.ResourcePack));

        Mods.Add(modjson.Name, modData);
    }
}
