using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Effects;
using Godot;

namespace ExtractIntoVoid.Modding.Effect;

public class MakeEffectBase : NodeEvent, IDisableCoreEvent
{
    public MakeEffectBase(Node node, IEffect coreEfect) : base(node)
    {
        CoreEfect = coreEfect;
    }

    public bool Disable { get; set; }
    public IEffect CoreEfect { get; set; }
    public EffectBase EffectBase { get; set; }
}
