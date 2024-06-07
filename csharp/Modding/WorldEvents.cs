using ExtractIntoVoid.Modding.BaseEvents;
using ExtractIntoVoid.Worlds;
using Godot;
using ModAPI.V2;
using System.Collections.Generic;

namespace ExtractIntoVoid.Modding;

public class WorldEvents
{
    #region MainWorld
    public class OnStartWorld : BaseEvent
    {
        public List<string> SpawnableNodes = new();
    }

    public class OnSpawnNode : MainWorldEvent, IDisableCoreEvent
    {
        public OnSpawnNode(Variant inputVariant, MainWorld world) : base(world)
        {
            InputVariant = inputVariant;
        }
        public Node ReturnerNode { get; set; }
        public Variant InputVariant { get; protected set; }
    }
    #endregion
    public class OnStartMapGen : BaseEvent, IDisableCoreEvent
    {
        public bool Disable { get; set; } = false;
    }
}
