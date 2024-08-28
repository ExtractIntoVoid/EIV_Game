using ExtractIntoVoid.Physics;

namespace ExtractIntoVoid.Modules
{
    public class PlayerModule
    {
        public BasePlayer Player;

        public InventoryModule Inventory;

        public EffectModule Effect;

        public HealthModule Health;

        public BaseChangingModule<int> Energy; // should be double?
        public BaseChangingModule<int> Hydration;

        public InventoryNodeModule InventoryNode;

        public PlayerModule(BasePlayer player) 
        {
            Player = player;
            Inventory = new(this);
            Effect = new(this);
            Health = new(0, 100);
            Energy = new(0, 100);
            Hydration = new(0, 100);
            InventoryNode = new()
            {
                CurrentPlayer = player
            };
            player.AddChild(InventoryNode);
            InventoryNode.SetMultiplayerAuthority(player.NetId);
        }
    }
}
