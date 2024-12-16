using EIV_Common.JsonStuff;
using EIV_JsonLib;
using EIV_JsonLib.Extension;
using ExtractIntoVoid.Modules;

namespace ExtractIntoVoid.Items;

public partial class HealingBase : UsableBase
{

    public override void OnUsingFinished()
    {
        var ihealing = UsableItem.As<Healing>();
        base.OnUsingFinished();
        this.Player.GetModuleNode<HealthModule>().Heal(ihealing.HealAmount, true);
        this.Player.GetModuleNode<EffectModule>().ApplyEffectFromItem(UsableItem);
    }
}
