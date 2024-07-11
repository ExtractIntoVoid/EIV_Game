using EIV_Common.JsonStuff;
using EIV_JsonLib.Classes;
using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Effects;
using ExtractIntoVoid.Modding.Effect;
using ModAPI.V2;
using System.Collections.Generic;

namespace ExtractIntoVoid.Modules
{
    public class EffectModule : IModule
    {
        public PlayerModule PlayerModule;

        List<EffectBase> Effects = [];

        public EffectModule(PlayerModule playerModule)
        {
            PlayerModule = playerModule;
        }


        public void ApplyEffectFromItem(IItem item)
        {
            var sideEffects = item.GetProperty<List<SideEffect>>("SideEffects");
            if (sideEffects.Count == 0)
                return;
            foreach (var sideEffect in sideEffects)
            {
                EffectApply(sideEffect, item);
            }
        }

        public void EffectApply(SideEffect sideEffect, IItem item)
        {
            var effect = EffectMaker.MakeNewEffect(sideEffect.EffectName);
            bool ret = effect.AppliedFrom.Contains(item.BaseID) || effect.AppliedFrom.Contains(item.ItemType);
            // Cannot Apply from it.
            if (!ret)
                return;

            MakeEffectBase makeEffectBase = new(sideEffect.EffectName, PlayerModule, effect);
            V2Manager.TriggerEvent(makeEffectBase);
            // This means we couldnt made EffectBase
            if (makeEffectBase.EffectBase == null)
                return;
            makeEffectBase.EffectBase.StartEffect(sideEffect.EffectTime, sideEffect.EffectStrength);
            Effects.Add(makeEffectBase.EffectBase);
        }

        public void DisableEffect()
        {

        }
    }
}
