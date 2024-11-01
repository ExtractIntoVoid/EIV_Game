using EIV_Common.JsonStuff;
using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Modules;

namespace ExtractIntoVoid.Items
{
    internal partial class Adrenalin : UsableBase
    {
        public override void _Ready()
        {
            base._Ready();
        }

        public override void OnUsingFinished()
        {
            var ihealing = UsableItem.As<IHealing>();
            base.OnUsingFinished();
            this.InventoryModule.Items.Remove(UsableItem);
            this.Player.GetModuleNode<HealthModule>().Heal(ihealing.HealAmount, true);
            //UsableItem
            this.Player.GetModuleNode<EffectModule>().ApplyEffectFromItem(UsableItem);
        }

    }
}
