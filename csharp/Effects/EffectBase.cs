using EIV_Common.Coroutines;
using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Modules;
using System.Collections.Generic;

namespace ExtractIntoVoid.Effects
{
    public abstract class EffectBase
    {
        Coroutine? TimeCoroutine;
        public IEffect CoreEffect { get; internal set; }
        public PlayerModule PlayerModules { get; internal set; }
        public EffectBase(IEffect effect, PlayerModule playerModules)
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

        public virtual void StopEffect()
        {
            if (TimeCoroutine == null)
                return;
            CoroutineWorkerNode.KillCoroutineInstance(TimeCoroutine.Value);
        }

        public virtual void EffectTick(int Strength)
        {
            if (CoreEffect.Strength.ApplyTo.Contains("Health"))
            {
                PlayerModules.Health.Damage(CoreEffect.Health.Negative * Strength, CoreEffect.Health.Cause);
                PlayerModules.Health.Heal(CoreEffect.Health.Positive * Strength, true);
            }
            if (CoreEffect.Strength.ApplyTo.Contains("Energy"))
            {
                PlayerModules.Energy.RemoveValue(CoreEffect.Energy.Negative * Strength);
                PlayerModules.Energy.AddValue(CoreEffect.Energy.Positive * Strength, false);
            }
        }

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
}
