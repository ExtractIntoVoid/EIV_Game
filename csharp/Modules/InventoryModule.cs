using EIV_Common.JsonStuff;
using EIV_JsonLib.Interfaces;
using System.Collections.Generic;

namespace ExtractIntoVoid.Modules
{
    public class InventoryModule : IModule
    {
        public PlayerModule Module;

        public InventoryModule(PlayerModule playerModules)
        {
            Module = playerModules;
        }

        public List<IItem> Items { get; set; } = [];

        IItem CurrentItem = null;

        public void Select(int index)
        {
            if (index > Items.Count)
                return;
            CurrentItem = Items[index];
            Select(CurrentItem);
        }

        private void Select(IItem selectedItem)
        {
            if (selectedItem == null)
                return;
            Module.InventoryNode.SelectItem(selectedItem);
        }

        /// <summary>
        /// Using the Selected Item
        /// </summary>
        public void Use()
        {
            if (CurrentItem.Is<IUsable>())
                Module.InventoryNode.Usable_Use();
            if (CurrentItem.Is<IGun>())
                Module.InventoryNode.Gun_Use();
        }

        /// <summary>
        /// Reload, or something.
        /// </summary>
        public void Special_Action()
        {

        }

        public void Cancel()
        {

        }
    }
}
