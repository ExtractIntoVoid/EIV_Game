namespace ExtractIntoVoid.Modules
{
    public class BaseChangingModule : IModule
    {
        public BaseChangingModule(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            CurrentValue = maxValue;
        }

        public virtual int MinValue { get; set; }
        public virtual int MaxValue { get; set; }
        public virtual int CurrentValue { get; set; }

        public virtual void AddValue(int value, bool EnableOverflow)
        {
            if (value < 0)
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

        public virtual void RemoveValue(int value) 
        {
            if (value < 0)
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
}
