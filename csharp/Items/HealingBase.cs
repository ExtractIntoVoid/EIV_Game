using EIV_Common.JsonStuff;
using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Modules;

namespace ExtractIntoVoid.Items;

public partial class HealingBase : UsableBase
{

    public override void OnUsingFinished()
    {
        var ihealing = UsableItem.As<IHealing>();
        base.OnUsingFinished();
        this.Player.GetModuleNode<HealthModule>().Heal(ihealing.HealAmount, true);
        this.Player.GetModuleNode<EffectModule>().ApplyEffectFromItem(UsableItem);
    }
}
