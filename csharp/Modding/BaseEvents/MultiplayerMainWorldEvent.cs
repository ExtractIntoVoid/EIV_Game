using ExtractIntoVoid.Worlds;
using ModAPI.V2;

namespace ExtractIntoVoid.Modding.BaseEvents;

public class MultiplayerMainWorldEvent : MainWorldEvent
{
    public MultiplayerMainWorldEvent() : base()
    {
        Id = 0;
    }
    public MultiplayerMainWorldEvent(long id, MainWorld mainWorld) : base(mainWorld)
    {
        Id = id;
    }
    public long Id { get; private set; }
}
