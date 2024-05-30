using Godot;
using ModAPI.V2;

namespace ExtractIntoVoid.Modding;

public class NodeEvent : BaseEvent
{
    public NodeEvent()
    {
        Node = null;
    }
    public NodeEvent(Node node)
    {
        Node = node;
    }
    public Node Node { get; private set; }
}
