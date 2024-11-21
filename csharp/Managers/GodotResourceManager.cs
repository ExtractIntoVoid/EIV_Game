using Godot;
using System.Collections.Generic;

namespace ExtractIntoVoid.Managers;

public static class GodotResourceManager
{
    public static Dictionary<string, string> Resources = new();

    static GodotResourceManager()
    {
        Resources = new()
        {

        };
    }

    public static Resource GetResource(string Name)
    {
        if (Resources.TryGetValue(Name, out string path))
        {
            return ResourceLoader.Load(path);
        }
        return null;

    }

    public static bool TryGetResourcePath(string Name, out string resource_path)
    {
        if (Resources.TryGetValue(Name, out string path))
        {
            resource_path = path;
            return true;
        }
        resource_path = null;
        return false;
    }
}