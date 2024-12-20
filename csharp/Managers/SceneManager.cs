﻿using Godot;
using System.Collections.Generic;

namespace ExtractIntoVoid.Managers;

public static class SceneManager
{
    public static Dictionary<string, string> Scenes = new();

    static SceneManager()
    {
        Scenes = new()
        {
            //worlds
            { "MainWorld", "res://scenes/World/MainWorld.tscn" },
            { "TestWorld", "res://scenes/World/TestWorld.tscn" },
#if CLIENT|| GAME
            //menus
            { "MainMenu", "res://scenes/Menu/MainMenu.tscn" },
            { "Settings", "res://scenes/Menu/Settings.tscn" },
            { "LoadingScreen", "res://scenes/Menu/LoadingScreen.tscn" },
            { "ConnectionScreen", "res://scenes/Menu/ConnectionScreen.tscn" },
            { "InputScene", "res://scenes/Menu/InputScene.tscn" },
            { "LobbyScene", "res://scenes/Menu/LobbyScene.tscn" },
            // soon
//            { "Inventory", "res://scenes/Menu/Inventory.tscn" },
//            { "Escape", "res://scenes/Menu/EscapeScene.tscn" },
#endif
            //others
            { "Player", "res://scenes/Character/Player.tscn" },
        };

    }

    public static PackedScene GetPackedScene(string Name)
    {
        if (Scenes.TryGetValue(Name, out string path))
        {
            if (ResourceLoader.Exists(path))
                return ResourceLoader.Load<PackedScene>(path);
        }
        return null;
    }

    public static bool TryGetPackedScene(string Name, out PackedScene scene)
    {
        if (Scenes.TryGetValue(Name, out string path))
        {
            if (ResourceLoader.Exists(path))
            {
                scene = ResourceLoader.Load<PackedScene>(path);
                return true;
            }
        }
        scene = null;
        return false;
    }
}
