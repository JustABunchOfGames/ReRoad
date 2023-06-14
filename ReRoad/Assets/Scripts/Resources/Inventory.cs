
using System.Collections.Generic;

namespace Resources
{
    public class Inventory
    {
        // List of resources
        private List<Resource> _inventory;

        // Keeping track of slots
        private int _usedInventorySlot;
        private int _inventoryMaxSize;

        // Revealed by Harvest or if it's the player inventory
        private bool _isRevealed = false;

        public Inventory(int inventoryMaxSize)
        {
            _inventory = new List<Resource>();
            _inventoryMaxSize = inventoryMaxSize;
            _usedInventorySlot = 0;
        }

        public List<Resource> GetInventory()
        {
            return _inventory;
        }

        public int GetUsedInventorySlot()
        {
            return _usedInventorySlot;
        }

        public int GetInventoryMaxSize()
        {
            return _inventoryMaxSize;
        }

        public void SetInventoryMaxSize(int inventoryMaxSize)
        {
            _inventoryMaxSize = inventoryMaxSize;
        }

        public void Reveal()
        {
            _isRevealed = true;
        }

        public bool isRevealed()
        {
            return _isRevealed;
        }

        public bool CanAddResource(Resource resource)
        {
            return resource.quantity + _usedInventorySlot <= _inventoryMaxSize;
        }

        public void AddResource(Resource resource)
        {
            _usedInventorySlot += resource.quantity;

            for (int i = 0; i < _inventory.Count; i++)
            {
                // If the resource already exist, add to its quantity
                if (_inventory[i].type == resource.type)
                {
                    _inventory[i].quantity += resource.quantity;
                    return;
                }
            }

            // If the resource don't exist already in inventory, add it
            _inventory.Add(resource);
        }

        public bool CanRemoveResource(Resource resource)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].type == resource.type)
                    return _inventory[i].quantity > 0;
            }
            return false;
        }

        public void RemoveResource(Resource resource)
        {
            _usedInventorySlot -= resource.quantity;

            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].type == resource.type)
                {
                    if (_inventory[i].quantity > resource.quantity)
                        _inventory[i].quantity -= resource.quantity;
                    else
                        _inventory.RemoveAt(i);
                    return;
                }
            }
        }
    }
}