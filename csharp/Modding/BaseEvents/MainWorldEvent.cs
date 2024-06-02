using ExtractIntoVoid.Worlds;
using ModAPI.V2;

namespace ExtractIntoVoid.Modding.BaseEvents;

public class MainWorldEvent : BaseEvent, IDisableCoreEvent
{
    public MainWorldEvent()
    {
        World = null;
    }
    public MainWorldEvent(MainWorld world)
    {
        World = world;
    }
    public MainWorld World { get; private set; }
    public bool Disable { get; set; } = false;
}
