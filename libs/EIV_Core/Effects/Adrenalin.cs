using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Effects;
using ExtractIntoVoid.Modules;
using Godot;

namespace EIV_Core.Effects;

public partial class Adrenalin : EffectBase
{
    public Adrenalin(IEffect effect, Node parentNode) : base(effect, parentNode)
    {
    }

    public override void EffectTick(int Strength)
    {
        base.EffectTick(Strength);
        // Somehow show in ui??
        //PlayerModules.Player.Camera.AddChild();
    }
}
