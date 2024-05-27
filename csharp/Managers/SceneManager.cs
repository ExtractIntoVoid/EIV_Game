using Godot;
using System.Collections.Generic;

namespace ExtractIntoVoid.Managers;

public class SceneManager
{
    public Dictionary<string, string> Scenes = new();

    public SceneManager()
    {
        Scenes = new()
        {
            //worlds
            { "TestWorld", "res://scenes/World/main_world.tscn" },
            { "MainWorld", "res://scenes/World/Worlds.tscn" },
            //menus
            { "MainMenu", "res://scenes/Menus/MainMenu.tscn" },
            { "Inventory", "res://scenes/Menus/Inventory.tscn" },
            { "Escape", "res://scenes/Menus/EscapeScene.tscn" },
            //others
            { "Player", "res://scenes/Player.tscn" },
        };

    }

    public PackedScene GetPackedScene(string Name)
    {
        if (Scenes.TryGetValue(Name, out string path))
        {
            return ResourceLoader.Load<PackedScene>(path);
        }
        return null;
    }

    public bool TryGetPackedScene(string Name, out PackedScene scene)
    {
        if (Scenes.TryGetValue(Name, out string path))
        {
            scene = ResourceLoader.Load<PackedScene>(path);
            return true;
        }
        scene = null;
        return false;
    }
}
