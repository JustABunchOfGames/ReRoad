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

        private Player.PlayerData _player;

        public void Setup(Player.PlayerData player)
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
    }
}