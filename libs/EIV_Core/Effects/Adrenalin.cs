using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Effects;
using ExtractIntoVoid.Modules;

namespace EIV_Core.Effects
{
    public class Adrenalin : EffectBase
    {
        public Adrenalin(IEffect effect, PlayerModule playerModules) : base(effect, playerModules)
        {
        }

        public override void EffectTick(int Strength)
        {
            base.EffectTick(Strength);
            // Somehow show in ui??
            //PlayerModules.Player.Camera.AddChild();
        }
    }
}
