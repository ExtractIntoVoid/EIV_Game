using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Effects;
using ExtractIntoVoid.Modules;
using ModAPI.V2;

namespace ExtractIntoVoid.Modding.Effect
{
    public class MakeEffectBase : BaseEvent, IDisableCoreEvent
    {
        public MakeEffectBase(PlayerModule playerModule, IEffect coreEfect) 
        {
            PlayerModule = playerModule;
            CoreEfect = coreEfect;
        }

        public bool Disable { get; set; }
        public PlayerModule PlayerModule { get; set; }
        public IEffect CoreEfect { get; set; }
        public EffectBase EffectBase { get; set; }
    }
}
