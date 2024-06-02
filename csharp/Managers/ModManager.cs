using ExtractIntoVoid.Data;
using Godot;
using ModAPI;
using ModAPI.ModLoad;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ExtractIntoVoid.Managers
{
    internal partial class ModManager : Node
    {
        public Dictionary<string, ModData> Mods;

        public static readonly IReadOnlyList<string> ModLocations =
        [
            Path.Combine(Path.GetDirectoryName(OS.GetExecutablePath()), ProjectSettings.GlobalizePath("res://mods")),
            ProjectSettings.GlobalizePath("user://mods"),
        ];

        public override void _Ready()
        {
            Mods = new();
            this.CallDeferred(nameof(this.LoadAllMods));
        }

        public void LoadAllMods()
        {
            Debugger.Enabled = true;
            MainLoader.Init();
            // Adding ourself as main.
            MainLoader.SetMainModAssembly(typeof(ModManager).Assembly);
            // Adding shared stuff into ALC
            var shared = Directory.GetFiles(Path.GetDirectoryName(typeof(ModManager).Assembly.Location), "*.dll");
            foreach ( var sharedFile in shared) 
            {
                if (sharedFile.Contains("EIV_Game"))
                    continue;
                if (sharedFile.Contains("GodotSharp"))
                    continue;
                if (sharedFile.Contains("ModAPI"))
                    continue;
                MainLoader.AddSharedAssembly(sharedFile);
            }
            foreach (var item in ModLocations)
            {
                GD.Print(item);
                if (!Directory.Exists(item))
                    continue;
                string[] modDirectories = Directory.GetDirectories(item);
                foreach (var mod in modDirectories)
                {
                    if (mod == "Dependencies")
                    {
                        LoadDependecies(mod);
                        continue;
                    }

                    LoadMod(mod);
                }
            }
        }

        void LoadMod(string modDir)
        {
            if (!File.Exists(Path.Combine(modDir, "Mod.json")))
            {
                GD.Print("Mod.json not found skipping this dir");
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
            MainLoader.LoadPlugin(Path.Combine(modDir, modData.AssemblyName));
            ProjectSettings.LoadResourcePack(Path.Combine(modDir, modData.ResourcePack));

            Mods.Add(modjson.Name, modData);
        }


        void LoadDependecies(string path)
        {
            foreach (var dll in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
            {
                MainLoader.AddSharedAssembly(dll);
            }
        }
    }
}
