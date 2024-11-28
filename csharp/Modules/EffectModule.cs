using EIV_Common.JsonStuff;
using EIV_JsonLib;
using EIV_JsonLib.Base;
using ExtractIntoVoid.Effects;
using ExtractIntoVoid.Modding.Effect;
using Godot;
using ModAPI.V2;
using System.Collections.Generic;
using System.Linq;

namespace ExtractIntoVoid.Modules;

public partial class EffectModule : Node, IModule
{
    internal List<EffectBase> Effects = [];

    public void ApplyEffectFromItem(ItemBase item)
    {
        var sideEffects = item.GetProperty<List<SideEffect>>("SideEffects");
        if (sideEffects.Count == 0)
            return;
        foreach (var sideEffect in sideEffects)
        {
            EffectApply(sideEffect, item);
        }
    }



    public void EffectApply(SideEffect sideEffect, ItemBase item)
    {
        var effect = EffectMaker.MakeNewEffect(sideEffect.EffectName);
        if (effect == null)
            return;
        // Cannot Apply from it.
        if (!effect.AppliedFrom.Contains(item.BaseID) || effect.AppliedFrom.Contains(item.ItemType))
            return;

        if (effect.Strength.Max < sideEffect.EffectStrength)
            return;
        if (effect.Strength.Min > sideEffect.EffectStrength)
            return;

        MakeEffectBase makeEffectBase = new(this, effect);
        V2Manager.TriggerEvent(makeEffectBase);
        // This means we couldnt made EffectBase
        if (makeEffectBase.EffectBase == null)
            return;
        makeEffectBase.EffectBase.StartEffect(sideEffect.EffectTime, sideEffect.EffectStrength);
        Effects.Add(makeEffectBase.EffectBase);
    }


    public void EffectApply(string EffectName)
    {
        var effect = EffectMaker.MakeNewEffect(EffectName);
        if (effect == null)
            return;
        EffectApply(effect, effect.Time.Initial, effect.Strength.Min);
    }

    public void EffectApply(Effect effect, double time, int strength)
    {
        if (effect == null)
            return;
        MakeEffectBase makeEffectBase = new(this, effect);
        V2Manager.TriggerEvent(makeEffectBase);
        // This means we couldnt made EffectBase
        if (makeEffectBase.EffectBase == null)
            return;
        makeEffectBase.EffectBase.StartEffect(time, strength);
        Effects.Add(makeEffectBase.EffectBase);
    }

    public void DisableEffect(string EffectName)
    {
        var effect = Effects.Where(x=>x.CoreEffect.EffectID == EffectName).SingleOrDefault();
        if (effect == null)
            return;
        effect.StopEffect();
    }

    public List<string> GetEffectNames() => Effects.Select(x=>x.CoreEffect.EffectID).ToList();

}
