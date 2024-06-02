#if SERVER || GAME
using ExtractIntoVoid.Controllers;
using ExtractIntoVoid.Extensions;
using ExtractIntoVoid.Managers;
using Godot;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExtractIntoVoid.Server
{
    public static class MapManager
    {
        public static List<string> Maps = new List<string>();
        public static void LoadMapList()
        {
            var PlayableMaps = INIController.Read(GameManager.INI, "Maps", "PlayableMaps");
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
            var RandomizeMaps = INIController.Read(GameManager.INI, "Maps", "RandomizeMaps");
            if (RandomizeMaps == "0")
            {
                return Maps.First();
            }
            if (RandomizeMaps == "1")
            {
                return Maps.GetRandom();
            }
            if (RandomizeMaps == "2")
            {
                //randomization
            }
            return string.Empty;
        }
    }
}
#endif