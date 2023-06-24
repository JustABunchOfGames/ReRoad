using Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Resources
{
    public class Inventory
    {
        // List of resources
        // private List<Resource> _inventory;
        private Dictionary<ResourceType, Resource> _inventory;

        // Keeping track of slots
        private int _usedInventorySlot;
        private int _inventoryMaxSize;

        // Revealed by Harvest or if it's the player inventory
        private bool _isRevealed = false;

        public Inventory(int inventoryMaxSize)
        {
            _inventory = new Dictionary<ResourceType, Resource>();
            _inventoryMaxSize = inventoryMaxSize;
            _usedInventorySlot = 0;
        }

        public Inventory(int inventoryMaxSize, List<Resource> resources)
        {
            _inventory = new Dictionary<ResourceType, Resource>();
            _inventoryMaxSize = inventoryMaxSize;
            _usedInventorySlot = 0;

            foreach(Resource resource in resources)
            {
                AddResource(resource);
            }
        }

        // Mostly used for display on UI
        public List<Resource> GetInventory()
        {
            return _inventory.Values.ToList();
        }

        public (int, int) GetInventorySlot()
        {
            return (_usedInventorySlot, _inventoryMaxSize);
        }

        public void IncreaseInventoryMaxSize(int increase)
        {
            _inventoryMaxSize += increase;
        }

        public void Reveal()
        {
            _isRevealed = true;
        }

        public bool isRevealed()
        {
            return _isRevealed;
        }

        public bool Give1To(Inventory inventoryToGive, ResourceType type)
        {
            Resource resource = new Resource(type, 1);

            // If the exchange can be done
            if (CanRemoveResource(resource) && inventoryToGive.CanAddResource(resource))
            {
                // Apply to inventory
                RemoveResource(resource);
                inventoryToGive.AddResource(resource);
                return true;
            }
            return false;
        }

        public bool CanAddResource(Resource resource)
        {
            return resource.quantity + _usedInventorySlot <= _inventoryMaxSize;
        }

        private void AddResource(Resource resource)
        {
            _usedInventorySlot += resource.quantity;

            bool exist = _inventory.ContainsKey(resource.type);

            if (exist)
                _inventory[resource.type].quantity += resource.quantity;
            else
                _inventory.Add(resource.type, resource);
        }

        private bool CanRemoveResource(Resource resource)
        {
            return _inventory.ContainsKey(resource.type) && _inventory[resource.type].quantity >= resource.quantity;
        }

        private void RemoveResource(Resource resource)
        {
            _usedInventorySlot -= resource.quantity;

            if (_inventory[resource.type].quantity > resource.quantity)
                _inventory[resource.type].quantity -= resource.quantity;
            else
                _inventory.Remove(resource.type);
        }

        public static Inventory ConcatInventories(Inventory inventory1, Inventory inventory2)
        {
            Inventory inventory = new Inventory(100000);

            foreach (Resource resource in inventory1.GetInventory())
            {
                inventory.AddResource(resource);
            }

            foreach (Resource resource in inventory2.GetInventory())
            {
                inventory.AddResource(resource);
            }
            return inventory;
        }

        public bool CanPayCost(List<Resource> resources)
        {
            bool canPay = true;
            foreach(Resource resource in resources)
            {
                canPay = CanRemoveResource(resource);

                if (!canPay)
                    break;
            }
            return canPay;
        }

        public List<Resource> PayCost(List<Resource> resources)
        {
            foreach(Resource resource in resources)
            {
                bool exist = _inventory.ContainsKey(resource.type);
                if (exist)
                {
                    int cost = _inventory[resource.type].quantity - resource.quantity;
                    
                    if (cost >= 0)
                    {
                        resources.Remove(resource);
                        _inventory[resource.type].quantity -= resource.quantity;
                    }
                    else
                    {
                        resource.quantity -= _inventory[resource.type].quantity;
                        _inventory.Remove(resource.type);
                    }
                }
            }
            return resources;
        }
    }
}