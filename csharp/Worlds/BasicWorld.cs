using Godot;
using ModAPI.V2;
using System;
using System.Collections.Generic;
using static ExtractIntoVoid.Modding.WorldEvents;

namespace ExtractIntoVoid.Worlds;

public abstract partial class BasicWorld : Node3D
{
    /// <summary>
    /// Map Name.
    /// </summary>
    public abstract string MapName { get; }

    /// <summary>
    /// Minimum players when the game should start.
    /// </summary>
    public abstract int MinPlayerCount { get; }

    /// <summary>
    /// Maximum player count for this map.
    /// </summary>
    public abstract int MaxPlayerCount { get; }

    /// <summary>
    /// List of points where the players could spawn.
    /// </summary>
    public List<Node3D> SpawnPoints = new();

    /// <summary>
    /// A seed for this map
    /// </summary>
    public int Seed { get; private set; }

    public override void _Ready()
    {
        foreach (var item in this.GetTree().GetNodesInGroup("SpawnPoints"))
        {
            if (item is Node3D node && node != null)
                SpawnPoints.Add(node);
        }
#if SERVER || GAME
        Seed = Random.Shared.Next();
        //start map gen (if something like that exists)
        V2Manager.TriggerEvent(new OnStartMapGen() { Seed = Seed });
        Rpc(MethodName.SeedSync, Seed);
#endif
    }

    [Rpc]
    public void SeedSync(int seed)
    {
        Seed = seed;
    }
}
