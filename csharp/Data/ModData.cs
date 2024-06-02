using System.Collections.Generic;

namespace ExtractIntoVoid.Data
{
    internal class ModData
    {
        public string AssemblyName;
        // use internal name here!
        public string ResourcePack;
#if SERVER
        public string SaveDir;
#endif
        public ModJson ModJson;
        public Dictionary<string, string> Hashes;
    }

    internal class ModJson
    {
        public string Name = string.Empty;
        public string InternalName = string.Empty;
        public string Authors = string.Empty;
        //  Using SemanticVersioning!
        public string Version = string.Empty;
        //  Internal Name | Version (* if you dont care about it) - WITH EIV.
        // For C# Dependencies just drop those into /Mods/Dependencies folder.
        public Dictionary<string, string> Dependencies = new();
    }
}
