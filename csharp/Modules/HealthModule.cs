using ExtractIntoVoid.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractIntoVoid.csharp.Modules
{
    public class HealthModule : BaseChangingModule
    {
        public HealthModule(int minValue, int maxValue) : base(minValue, maxValue)
        {
        }

        public void Damage(int value, string Cause)
        {
            this.CurrentValue -= value;
            if (this.CurrentValue == this.MinValue)
            {
                OnDeath?.Invoke(null, Cause);
                // play Death
            }
        }

        public void Heal(int value)
        {

        }

        public EventHandler<string> OnDeath;
    }
}
