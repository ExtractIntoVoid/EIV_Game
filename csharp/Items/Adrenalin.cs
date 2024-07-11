using EIV_Common.JsonStuff;
using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Modules.Inventory.Items.Remove(UsableItem);
            Modules.Health.Heal(ihealing.HealAmount, true);
            //UsableItem
            Modules.Effect.ApplyEffectFromItem(UsableItem, ApplyTo);
        }

    }
}
