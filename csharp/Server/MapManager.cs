#if SERVER || GAME
using EIV_Common;
using EIV_Common.Extensions;
using ExtractIntoVoid.Managers;
using Godot;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExtractIntoVoid.Server
{
    public class MapManager
    {
        public static List<string> Maps = new List<string>();
        public static void LoadMapList()
        {
            var PlayableMaps = ConfigINI.Read(BuildDefined.INI, "Maps", "PlayableMaps");
            if (PlayableMaps.Contains(","))
                Maps.AddRange(PlayableMaps.Split(","));
            else
                Maps.Add(PlayableMaps);
        }

        public static string RandomizeMap()
        {
            //  If there is no map, we just simply return empty.
            if (Maps.Count == 0)
                return string.Empty;
            var RandomizeMaps = ConfigINI.Read<int>(BuildDefined.INI, "Maps", "RandomizeMaps");
            if (RandomizeMaps == 0)
            {
                return Maps.First();
            }
            if (RandomizeMaps == 1)
            {
                return Maps.GetRandom();
            }
            if (RandomizeMaps == 2)
            {
                //randomization
            }
            return string.Empty;
        }
    }
}
#endif
