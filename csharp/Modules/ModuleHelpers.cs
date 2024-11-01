using Godot;
using System.Linq;

namespace ExtractIntoVoid.Modules;

public static class ModuleHelpers
{
    public static T GetModuleNode<T>(this Node node) where T : Node, IModule, new()
    {
        return (T)node.GetChildren().FirstOrDefault(x => x is T);
    }

    public static bool TryGetModuleNode<T>(this Node node, out T out_t) where T : Node, IModule, new()
    {
        out_t = default;
        out_t = (T)node.GetChildren().FirstOrDefault(x => x is T);
        return out_t != null;
    }

    public static void AddModuleNode<T>(this Node node, T module) where T : Node, IModule, new()
    {
        module.SetMultiplayerAuthority(node.GetMultiplayerAuthority());
        node.AddChild(module);
    }

    public static void RemoveModuleNode<T>(this Node node) where T : Node, IModule, new()
    {
        var NodeList = node.GetChildren().Where(x=>x is T).ToList();
        foreach(var Node in NodeList)
        {
            node.RemoveChild(Node);
        }
    }
}
