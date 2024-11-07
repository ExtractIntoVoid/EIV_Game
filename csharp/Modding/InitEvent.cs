using ModAPI.V2;

namespace ExtractIntoVoid.Modding;

public class InitEvent : BaseEvent, IDisableCoreEvent
{
    public bool Disable { get; set; }
}
