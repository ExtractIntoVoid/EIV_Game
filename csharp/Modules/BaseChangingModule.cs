using Godot;
using System.Numerics;

namespace ExtractIntoVoid.Modules;

public partial class BaseChangingModule<T> : Node, IModule where T : IMinMaxValue<T>, INumber<T>
{
    public BaseChangingModule()
    {
        MinValue = T.MinValue;
        MaxValue = T.MaxValue;
        CurrentValue = T.MaxValue;
    }

    public BaseChangingModule(T minValue, T maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        CurrentValue = maxValue;
    }

    public BaseChangingModule(T minValue, T maxValue, T currentValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        CurrentValue = currentValue;
    }

    public virtual T MinValue { get; set; }
    public virtual T MaxValue { get; set; }
    public virtual T CurrentValue { get; set; }

    public virtual void AddValue(T value, bool EnableOverflow)
    {
        if (value < T.Zero)
            return;
        var newValue = CurrentValue + value;
        if (newValue > this.MaxValue)
        {
            if (EnableOverflow)
            {
                this.MaxValue = newValue;
                this.CurrentValue = newValue;
            }
        }
        else
        {
            this.CurrentValue = newValue;
        }
    }

    public virtual void RemoveValue(T value) 
    {
        if (value < T.Zero)
            return; 
        this.CurrentValue -= value;
        if (this.CurrentValue == this.MinValue)
        {
            OnMinimum();
        }
    }

    public virtual void OnMinimum()
    {

    }
}
