using Godot;
using ModAPI.V2;
using System.Collections.Generic;
using System.Linq;
using static ExtractIntoVoid.Modding.WorldEvents;

namespace ExtractIntoVoid.Worlds;

public partial class BasicWorld : Node3D
{
    public List<Node3D> SpwanPoints = new();

    public override void _Ready()
    {
        var spawnpointers = this.GetChildren().Where(x=>x.IsInGroup("SpawnPoints"));
        foreach (var item in spawnpointers)
        {
            GD.Print(item.Name);
            if (item is Node3D node && node != null)
                SpwanPoints.Add(node);
        }
        //start map gen (if something like that exists)
        V2EventManager.TriggerEvent(new OnStartMapGen());
    }


    public virtual void Spawn(long id)
    {
        GD.Print("Spawn",id);
    }
}
