using System;

namespace ExtractIntoVoid.Modules;

public partial class HealthModule : BaseChangingModule<int>
{
    public HealthModule() : base(0, 100)
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
