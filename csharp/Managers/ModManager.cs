using ExtractIntoVoid.Data;
using ExtractIntoVoid.Modding;
using Godot;
using ModAPI;
using ModAPI.ModLoad;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExtractIntoVoid.Managers
{
    internal partial class ModManager : Node
    {
        public Dictionary<string, ModData> Mods;

        public event Action AllModsLoaded;

        public static readonly IReadOnlyList<string> ModLocations =
        [
            Path.Combine(Path.GetDirectoryName(OS.GetExecutablePath()), ProjectSettings.GlobalizePath("res://mods")),
            ProjectSettings.GlobalizePath("user://mods"),
        ];

        static readonly List<string> SkipModLoad = 
        [
            "EIV_Game.dll", "GodotSharp.dll", "ModAPI.dll"
        ];

        public override void _Ready()
        {
            Mods = new();
            this.CallDeferred("LoadAllMods");
        }

        public void LoadAllMods()
        {
            Debugger.Enabled = true;
            ModLoad2.Init();
            ModLoad2.LoadDependencies();
            /*
            MainLoader.Init();
            // Adding ourself as main.
            MainLoader.SetMainModAssembly(typeof(ModManager).Assembly);
            
            // Adding all DLL that in our data into ALC (it will skip if there is not a C# DLL or cannot load it.)
            var shared = Directory.GetFiles(Path.GetDirectoryName(typeof(ModManager).Assembly.Location), "*.dll");
            foreach ( var sharedFile in shared) 
            {
                if (SkipModLoad.Contains(sharedFile))
                    continue;
                MainLoader.AddSharedAssembly(sharedFile);
            }
            MainLoader.LoadDependencies();
            */
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
            // MainLoader.LoadPlugin(Path.Combine(modDir, modData.AssemblyName));
            ModLoad2.LoadMod(Path.Combine(modDir, modData.AssemblyName));
            ProjectSettings.LoadResourcePack(Path.Combine(modDir, modData.ResourcePack));

            Mods.Add(modjson.Name, modData);
        }
    }
}
