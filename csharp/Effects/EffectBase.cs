using EIV_Common.Coroutines;
using EIV_JsonLib;
using ExtractIntoVoid.Modules;
using Godot;
using System.Collections.Generic;

namespace ExtractIntoVoid.Effects;

public abstract partial class EffectBase : Node
{
    Coroutine? TimeCoroutine;
    public Effect CoreEffect { get; internal set; }
    public Node ParentNode { get; internal set; }

    public EffectBase(Effect effect, Node parentNode)
    {
        CoreEffect = effect;
        ParentNode = parentNode;
    }

    public virtual void StartEffect()
    {
        TimeCoroutine = CoroutineWorkerNode.StartCoroutine(TimeStuff(CoreEffect.Time.Initial, CoreEffect.Strength.Min), CoroutineType.Process);
    }

    public virtual void StartEffect(double Seconds, int Strength)
    {
        TimeCoroutine = CoroutineWorkerNode.StartCoroutine(TimeStuff(Seconds, Strength), CoroutineType.Process);
    }

    public virtual void StopEffect()
    {
        if (TimeCoroutine == null)
            return;
        CoroutineWorkerNode.KillCoroutineInstance(TimeCoroutine.Value);
    }

    public virtual void EffectTick(int Strength)
    {
        if (CoreEffect.Strength.ApplyTo.Contains("Health") && ParentNode.TryGetModuleNode(out HealthModule healthModule))
        {
            healthModule.Damage(CoreEffect.Health.Negative * Strength, CoreEffect.Health.Cause);
            healthModule.Heal(CoreEffect.Health.Positive * Strength, true);
        }
        if (CoreEffect.Strength.ApplyTo.Contains("Energy") && ParentNode.TryGetModuleNode(out EnergyModule energyModule))
        {
            energyModule.RemoveValue(CoreEffect.Energy.Negative * Strength);
            energyModule.AddValue(CoreEffect.Energy.Positive * Strength, false);
        }
    }

    private IEnumerator<double> TimeStuff(double InitialTime, int Strength)
    {
        yield return CoreEffect.Time.WaitUntilApply;
        var time = InitialTime;
        yield return CoroutineWorkerNode.WaitUntilZero(
        () => 
        {
            EffectTick(Strength);
            time--;
            return time;
        });
        TimeCoroutine = null;
    }
}
