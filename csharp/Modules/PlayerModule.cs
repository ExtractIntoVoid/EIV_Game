using ExtractIntoVoid.csharp.Modules;
using ExtractIntoVoid.Physics;

namespace ExtractIntoVoid.Modules
{
    public class PlayerModule
    {
        Player internal_player;

        public InventoryModule Inventory;
        public HealthModule Health;

        public BaseChangingModule Energy;
        public BaseChangingModule Hydration;
        public EffectModule Effect;

        public PlayerModule(Player player) 
        {
            internal_player = player;
            Inventory = new(this);
            Health = new(0, 100);
            Energy = new(0, 100);
            Hydration = new(0, 100);
            Effect = new(this);
        }
    }
}
