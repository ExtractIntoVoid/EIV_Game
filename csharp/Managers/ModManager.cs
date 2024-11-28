using EIV_DataPack;
using ExtractIntoVoid.Data;
using ExtractIntoVoid.Modding;
using Godot;
using ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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
            if (!Directory.Exists(item))
                continue;
            string[] modDirectories = Directory.GetDirectories(item);
            foreach (var mod in modDirectories)
            {
                if (mod.Contains("Dependencies"))
                    continue;
                GameManager.Instance.logger.Verbose($"Trying to load mod from Directory: {mod}");
                LoadMod(mod);
            }
        }
        RPC_EventManager.Load();
        AllModsLoaded?.Invoke();
    }

    void LoadMod(string modDir)
    {
        var eivpMods = Directory.GetFiles(modDir, ".eivp");
        foreach (var mod in eivpMods)
        {
            var dp = DatapackCreator.Read(mod);
            if (!dp.CanRead())
                continue;
            EIV_Common.ModManager.LoadAssets_Pack(dp.GetReader()!);
        }

        if (!File.Exists(Path.Combine(modDir, "Mod.json")))
        {
            GameManager.Instance.logger.Warning($"Mod.json not found skipping {modDir} dir");
            return;
        }

        var modjson = JsonSerializer.Deserialize<ModJson>(File.ReadAllText(Path.Combine(modDir, "Mod.json")));
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

        if (File.Exists(Path.Combine(modDir, modData.AssemblyName)))
            GameManager.Instance.logger.Information("LoadMod Success? " + MainLoader.LoadMod(Path.Combine(modDir, modData.AssemblyName)));
        if (File.Exists(Path.Combine(modDir, modData.ResourcePack)))
            GameManager.Instance.logger.Information("LoadResourcePack Success? " + ProjectSettings.LoadResourcePack(Path.Combine(modDir, modData.ResourcePack)));

        Mods.Add(modjson.Name, modData);
        // Initializing the mods
        ModAPI.V2.V2Manager.TriggerEvent(new InitEvent());
    }
}
