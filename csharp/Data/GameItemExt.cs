using EIV_JsonLib.Interfaces;
using Godot;

namespace ExtractIntoVoid
{
    public static class GameItemExt
    {
        public static bool HasValidAssetPath(this IItem item)
        {
            return FileAccess.FileExists(item.AssetPath);
        }
    }
}
