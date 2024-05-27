using Godot;
using System.Collections.Generic;

namespace ExtractIntoVoid.Managers;

public class GodotResourceManager
{
    public Dictionary<string, string> Resources = new();

    public GodotResourceManager()
    {
        Resources = new()
        {

        };

    }

    public Resource GetResource(string Name)
    {
        if (Resources.TryGetValue(Name, out string path))
        {
            return ResourceLoader.Load(path);
        }
        return null;

    }

    public bool TryGetResourcePath(string Name, out string resource_path)
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