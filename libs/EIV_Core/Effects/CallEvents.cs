using ExtractIntoVoid.Effects;
using ExtractIntoVoid.Modding.Effect;

namespace EIV_Core.Effects
{
    public class CallEvents
    {
        public static Dictionary<string, Type> ClassNameToType = new()
        {
            { "EIV_Core.Effects.Adrenalin", typeof(Adrenalin) }

        };

        public static void MakeEffectBase_EIV_CORE(MakeEffectBase makeEffectBase)
        {
            if (makeEffectBase.Disable)
                return;
            if (!ClassNameToType.TryGetValue(makeEffectBase.CoreEfect.UseClass, out var type))
                return;

            var cctor = type.GetConstructors()[0];
            var effectBase = (EffectBase)cctor.Invoke([makeEffectBase.CoreEfect, makeEffectBase.Node]);
            if (effectBase == null) 
                return;

            makeEffectBase.EffectBase = effectBase;
        }
    }
}
