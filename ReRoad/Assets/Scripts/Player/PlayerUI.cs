using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerUI : MonoBehaviour
    {
        private Player _player;

        [Header("PlayerInventoryText")]
        [SerializeField] private Text _inventorySizeText;
        [SerializeField] private ResourceDisplay _displayedPlayerResources;
        [SerializeField] private ResourceDisplay _displayedTileResources;

        private IEnumerator Start()
        {
            _player = GetComponent<Player>();

            while (_player.inventory == null)
                yield return new WaitForSeconds(0.5f);

            // Display Player slots
            _inventorySizeText.text = _player.inventory.GetUsedInventorySlot() + " / " + _player.inventory.GetInventoryMaxSize();

            // Display PlayerInventory
            DisplayInventory(_player.inventory, _displayedPlayerResources);

            // Display TileInventory for the tile the player is on
            DisplayInventory(_player.currentTile.inventory, _displayedTileResources);
        }

        private void DisplayInventory(Inventory inventory, ResourceDisplay resourceDisplay)
        {
            resourceDisplay.gameObject.SetActive(inventory.isRevealed());

            foreach (Resource resource in inventory.GetInventory())
            {
                resourceDisplay.SetQuantity(resource);
            }
        }

        // Called after an harvest or after the player moved
        public void UpdateTileInventory()
        {
            DisplayInventory(_player.currentTile.inventory, _displayedTileResources);
        }

        // Called from ExchangeResource, on buttons
        public void ExchangeResource(bool playerToTile, ResourceType type)
        {
            if (playerToTile)
                Exchange(_player.inventory, _player.currentTile.inventory, _displayedPlayerResources, _displayedTileResources, type);
            else
                Exchange(_player.currentTile.inventory, _player.inventory, _displayedTileResources, _displayedPlayerResources, type);
        }

        private void Exchange(
            Inventory fromInventory, Inventory toInventory,
            ResourceDisplay fromDisplay, ResourceDisplay toDisplay,
            ResourceType resourceType)
        {
            Resource resource = new Resource(resourceType, 1);
            
            // If the exchange can be done
            if (fromInventory.CanRemoveResource(resource) && toInventory.CanAddResource(resource))
            {
                // Apply to inventory
                fromInventory.RemoveResource(resource);
                toInventory.AddResource(resource);

                // Apply to display
                fromDisplay.SubstractQuantity(resource);
                toDisplay.AddQuantity(resource);

                // Update inventorySize
                _inventorySizeText.text = _player.inventory.GetUsedInventorySlot() + " / " + _player.inventory.GetInventoryMaxSize();
            }
        }

        // Called from ExchangeResource, on buttons
        public void FillResource(bool playerToTile, ResourceType type)
        {
            Resource resource = new Resource(type, 1);
            if (playerToTile)
            {
                while (_player.inventory.CanRemoveResource(resource))
                {
                    Exchange(_player.inventory, _player.currentTile.inventory, _displayedPlayerResources, _displayedTileResources, type);
                }
            }
            else
            {
                while(_player.inventory.CanAddResource(resource) && _player.currentTile.inventory.CanRemoveResource(resource))
                {
                    Exchange(_player.currentTile.inventory, _player.inventory, _displayedTileResources, _displayedPlayerResources, type);
                }
            }
        }
    }
}