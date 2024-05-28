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
        public string Name;
        public string InternalName;
        public string Authors;
        //  Using SemanticVersioning!
        public string Version;
        //  Internal Name | Version (* if you dont care about it) - WITH EIV.
        // For C# Dependencies just drop those into /Mods/Dependencies folder.
        public Dictionary<string, string> Dependencies;
    }
}
