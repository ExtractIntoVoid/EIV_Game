using ExtractIntoVoid.Extensions;
using Godot;
using ModAPI.V2;
using System.Collections.Generic;
using System.Linq;
using static ExtractIntoVoid.Modding.WorldEvents;

namespace ExtractIntoVoid.Worlds;

public partial class BasicWorld : Node3D
{
    public List<Node3D> SpawnPoints = new();

    public override void _Ready()
    {
        var spawnpointers = this.GetTree().GetNodesInGroup("SpawnPoints");
        foreach (var item in spawnpointers)
        {
            if (item is Node3D node && node != null)
                SpawnPoints.Add(node);
        }
        //start map gen (if something like that exists)
        V2Manager.TriggerEvent(new OnStartMapGen());
    }
}
