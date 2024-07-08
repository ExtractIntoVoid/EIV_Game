using EIV_Common.Coroutines;
using EIV_Common.JsonStuff;
using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Modules;
using System;
using System.Collections.Generic;

namespace ExtractIntoVoid.Effects
{
    public abstract partial class EffectsBase
    {
        Coroutine? TimeCoroutine;
        public IEffect CoreEffect { get; internal set; }
        public PlayerModule PlayerModules;
        public EffectsBase(IEffect effect, PlayerModule playerModules)
        {
            CoreEffect = effect;
            PlayerModules = playerModules;
        }

        public virtual void StartEffect(double Seconds, int Strength)
        {
            if (TimeCoroutine == null)
                return;
            TimeCoroutine = CoroutineWorkerNode.StartCoroutine(TimeStuff(Seconds, Strength), CoroutineType.Process);
        }

        public abstract void EffectTick(int Strength);

        private IEnumerator<double> TimeStuff(double InitialTime, int Strength)
        {
            yield return CoreEffect.Time.WaitUntilApply;
            var time = InitialTime;
            CoroutineWorkerNode.WaitUntilZero(
            () => 
            {
                EffectTick(Strength);
                time--;
                return time;
            });
            TimeCoroutine = null;
        }
    }

    public class Alcohol : EffectsBase
    {
        public Alcohol(IEffect effect, PlayerModule playerModules) : base(effect, playerModules)
        {

        }

        public override void EffectTick(int Strength)
        {
            PlayerModules.Health.CurrentValue -= CoreEffect.Health.Negative;
            PlayerModules.Health.CurrentValue += CoreEffect.Health.Positive;
        }
    }
}
