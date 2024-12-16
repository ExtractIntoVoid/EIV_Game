using EIV_JsonLib.Base;

namespace ExtractIntoVoid.Items;

public static class ItemExt
{
    public static bool HasValidAssetPath(this CoreItem item)
    {
        return Godot.FileAccess.FileExists(item.AssetPath) || Godot.ResourceLoader.Exists(item.AssetPath);
    }
}
