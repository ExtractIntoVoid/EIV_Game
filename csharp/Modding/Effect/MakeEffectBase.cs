using EIV_JsonLib;
using ExtractIntoVoid.Effects;
using Godot;

namespace ExtractIntoVoid.Modding.Effect;

public class MakeEffectBase : NodeEvent, IDisableCoreEvent
{
    public MakeEffectBase(Node node, EIV_JsonLib.Effect coreEfect) : base(node)
    {
        CoreEfect = coreEfect;
    }

    public bool Disable { get; set; }
    
    public EIV_JsonLib.Effect CoreEfect { get; set; }
    public EffectBase EffectBase { get; set; }
}
