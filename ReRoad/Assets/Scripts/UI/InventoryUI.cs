using Resources;
using UnityEngine;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {

        [Header("PlayerInventory")]
        [SerializeField] private InventoryDisplay _displayedPlayerResources;

        [Header("TileInventory")]
        [SerializeField] private InventoryDisplay _displayedTileResources;

        private Player.Player _player;

        public void Setup(Player.Player player)
        {
            _player = player;
        }

        public void UpdateInventory()
        {
            // Player inventory
            _displayedPlayerResources.gameObject.SetActive(_player.inventory.isRevealed());

            _displayedPlayerResources.UpdateDisplay(_player.inventory);

            // Tile inventory, tile the player is on
            _displayedTileResources.gameObject.SetActive(_player.currentTile.inventory.isRevealed());

            _displayedTileResources.UpdateDisplay(_player.currentTile.inventory);
        }

        // Called from ExchangeResource, on buttons
        public bool Give1Resource(bool playerToTile, ResourceType type)
        {
            bool isExchangeOk = false;

            if (playerToTile)
            {
                isExchangeOk = _player.inventory.Give1To(_player.currentTile.inventory, type);
            }
            else
            {
                isExchangeOk = _player.currentTile.inventory.Give1To(_player.inventory, type);
            }

            if (isExchangeOk)
                UpdateInventory();

            return isExchangeOk;
        }
    }
}