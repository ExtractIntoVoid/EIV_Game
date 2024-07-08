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
    }
}
