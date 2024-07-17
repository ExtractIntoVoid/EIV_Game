using EIV_Common.JsonStuff;
using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Items;
using ExtractIntoVoid.Physics;
using System.Collections.Generic;

namespace ExtractIntoVoid.Modules
{
    public class InventoryModule : IModule
    {
        public PlayerModule Player;

        public InventoryModule(PlayerModule playerModules)
        {
            Player = playerModules;
        }

        public List<IItem> Items { get; set; } = [];

        IItem CurrentItem = null;
        UsableBase CurrentUsable = null;


        public void Select(int index)
        {
            CurrentItem = Items[index];
            Select(CurrentItem);
        }

        private void Select(IItem selectedItem)
        {
            if (selectedItem == null)
                return;
            
        }

        public void Use()
        {
            if (CurrentItem.Is<IUsable>())
            {
                Usable_Use();
            }
        }

        /// <summary>
        /// Reload, or something.
        /// </summary>
        public void Special_Action()
        {

        }

        void Usable_Use()
        {
            var usable = CurrentItem.As<IUsable>();
            // replace herer with getting UsableBase from modding
            UsableBase usable_base = new Adrenalin();
            usable_base.UsableItem = usable;
            usable_base.UsingStart();
        }

        void Gun_Use()
        {
            var gun = CurrentItem.As<IGun>();
            gun.
        }

        public void Cancel()
        {

        }
    }
}
