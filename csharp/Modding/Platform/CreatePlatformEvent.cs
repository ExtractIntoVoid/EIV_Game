using EIV_Common.Platform;
using ModAPI.V2;

namespace ExtractIntoVoid.Modding.Platform;

public class CreatePlatformEvent : BaseEvent
{
    public IPlatform Platform { get; set; }
}
