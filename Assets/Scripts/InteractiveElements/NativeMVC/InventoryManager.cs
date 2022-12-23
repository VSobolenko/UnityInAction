using System.Collections.Generic;
using UnityEngine;

namespace InteractiveElements.NativeMVC
{
    public class InventoryManager : MonoBehaviour, IGameManager
    {
        public ManageStatus Status { get; private set; }

        private Dictionary<string, int> _items;
        public string equippedItem { get; private set; }
        
        public void Startup()
        {
            Debug.Log("Inventory manager starting...");
            _items = new Dictionary<string, int>();
            Status = ManageStatus.Started;
        }

        public void Setup(NetworkService service)
        {
            
        }

        private void DisplayItems()
        {
            var itemDisplay = "Items: ";
            foreach (var item in _items)
            {
                itemDisplay += item.Key + " (" + item.Value + ") ";
            }

            Debug.Log(itemDisplay);
        }

        public void AddItem(string itemName)
        {
            if (_items.ContainsKey(itemName))
            {
                _items[itemName] += 1;
            }
            else
            {
                _items[itemName] = 1;
            }
            DisplayItems();
        }

        public List<string> GetItemList()
        {
            return new List<string>(_items.Keys);
        }

        public int GetItemCount(string itemKey)
        {
            if (_items.ContainsKey(itemKey))
            {
                return _items[itemKey];
            }

            return 0;
        }

        public bool EquipItem(string itemName)
        {
            if (_items.ContainsKey(itemName) && equippedItem != itemName)
            {
                equippedItem = itemName;
                Debug.Log($"Equipped: {itemName}");
                return true;
            }

            equippedItem = null;
            Debug.Log("Unequipped");
            return false;
        }
        
        public bool ConsumeItem(string name) {
            if (_items.ContainsKey(name)) { 
                    _items[name]--;
                if (_items[name] == 0) { 
                    
                    _items.Remove(name);
                }
            } else { 
                    Debug.Log("cannot consume " + name);
                return false;
            }
            DisplayItems();
            return true;
        }
    }
}