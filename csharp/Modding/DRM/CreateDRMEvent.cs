using EIV_Common.DRM;
using ModAPI.V2;

namespace ExtractIntoVoid.Modding.DRM;

public class CreateDRMEvent : BaseEvent
{

    public IDRM DRM { get; set; }
}
