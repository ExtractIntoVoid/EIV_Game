using Godot;
using ModAPI.V2;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public abstract int MaxPlayerCount { get; }

    public List<Node3D> SpawnPoints = new();

    public int Seed { get; private set; }

    public override void _Ready()
    {
        foreach (var item in this.GetTree().GetNodesInGroup("SpawnPoints"))
        {
            if (item is Node3D node && node != null)
                SpawnPoints.Add(node);
        }
        Seed = Random.Shared.Next();
        //start map gen (if something like that exists)
        V2Manager.TriggerEvent(new OnStartMapGen() { Seed = Seed });
    }
}
