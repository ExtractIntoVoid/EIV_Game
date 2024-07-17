using ExtractIntoVoid.Physics;

namespace ExtractIntoVoid.Modules
{
    public class PlayerModule
    {
        public Player Player;

        public InventoryModule Inventory;
        public HealthModule Health;

        public BaseChangingModule Energy;
        public BaseChangingModule Hydration;
        public EffectModule Effect;

        public PlayerModule(Player player) 
        {
            Player = player;
            Inventory = new(this);
            Health = new(0, 100);
            Energy = new(0, 100);
            Hydration = new(0, 100);
            Effect = new(this);
        }
    }
}
