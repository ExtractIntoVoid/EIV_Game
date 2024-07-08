using EIV_Common.JsonStuff;
using EIV_JsonLib.Classes;
using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Effects;
using System.Collections.Generic;

namespace ExtractIntoVoid.Modules
{
    public class EffectModule : IModule
    {
        public PlayerModule PlayerModule;

        List<EffectsBase> Effects = [];

        public EffectModule(PlayerModule playerModule)
        {
            PlayerModule = playerModule;
        }


        public void ApplyEffectFromItem(IItem item, string To)
        {
            var sideEffects = item.GetProperty<List<SideEffect>>("SideEffects");
            if (sideEffects.Count == 0)
                return;
            foreach (var sideEffect in sideEffects)
            {
                EffectApply(sideEffect, item, To);
            }
        }

        public void EffectApply(SideEffect sideEffect, IItem item, string To)
        {
            var effect = EffectMaker.MakeNewEffect(sideEffect.EffectName);
            bool ret = effect.AppliedFrom.Contains(item.BaseID) || effect.AppliedFrom.Contains(item.ItemType);
            // Cannot Apply from it.
            if (!ret)
                return;
            // Cannot Apply to it.
            if (!effect.AppliedTo.Contains(To))
                return;
            // replace this!
            EffectsBase effectsBase = new Alcohol(effect, PlayerModule);
            effectsBase.StartEffect(sideEffect.EffectTime, sideEffect.EffectStrength);
            Effects.Add(effectsBase);
        }
    }
}
