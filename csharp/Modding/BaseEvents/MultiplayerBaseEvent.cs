using ModAPI.V2;

namespace ExtractIntoVoid.Modding;

public class MultiplayerBaseEvent : BaseEvent
{
    public MultiplayerBaseEvent()
    {
        Id = 0;
    }
    public MultiplayerBaseEvent(long id)
    {
        Id = id;
    }
    public long Id { get; private set; }
}
