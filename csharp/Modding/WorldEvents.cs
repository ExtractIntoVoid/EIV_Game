using Godot;
using ModAPI.V2;
using System.Collections.Generic;

namespace ExtractIntoVoid.Modding
{
    public class WorldEvents
    {
        public class OnStartWorld : BaseEvent
        {
            public List<string> SpawnableNodes = new();
        }

        public class SpawnNode : NodeEvent, IDisableCoreEvent
        {
            public SpawnNode(Node node) : base(node) { }
            public bool Disable { get; set; } = false;
        }

        public class DeSpawnNode : NodeEvent, IDisableCoreEvent
        {
            public DeSpawnNode(Node node) : base(node) { }
            public bool Disable { get; set; } = false;
        }
    }
}
