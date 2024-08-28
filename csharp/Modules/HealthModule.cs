using System;

namespace ExtractIntoVoid.Modules
{
    public class HealthModule : BaseChangingModule<int>
    {
        public HealthModule(int minValue, int maxValue) : base(minValue, maxValue)
        {
        }

        string _LastCause;

        public void Damage(int value, string Cause)
        {
            _LastCause = Cause;
            base.RemoveValue(value);
        }

        public void Heal(int value, bool EnableOverHeal)
        {
            base.AddValue(value, EnableOverHeal);
        }

        public override void OnMinimum()
        {
            OnDeath?.Invoke(this, _LastCause);
        }

        public EventHandler<string> OnDeath;
    }
}
