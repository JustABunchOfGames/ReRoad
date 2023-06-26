using Player;
using Resources;
using UI;
using UnityEngine;

namespace Action
{
    public class ActionManageResource : MonoBehaviour
    {
        // PlayerData for inventory and tile inventory
        PlayerData _player;

        // Updating UI
        private UIManager _uiManager;

        public void Setup(PlayerData player, UIManager uiManager)
        {
            _player = player;
            _uiManager = uiManager;
        }

        public void ExchangeResource(bool playerToTile, bool fill, ResourceType resourceType)
        {
            bool isExchangeOK = false;

            if (playerToTile)
            {
                if (fill)
                {
                    do
                    {
                        isExchangeOK = _player.inventory.Give1To(_player.currentTile.inventory, resourceType);
                    }
                    while (isExchangeOK);
                }
                else
                {
                    isExchangeOK = _player.inventory.Give1To(_player.currentTile.inventory, resourceType);
                }
            }
            else
            {
                if (fill)
                {
                    do
                    {
                        isExchangeOK = _player.currentTile.inventory.Give1To(_player.inventory, resourceType);
                    }
                    while (isExchangeOK);
                }
                else
                {
                    isExchangeOK = _player.currentTile.inventory.Give1To(_player.inventory, resourceType);
                }
            }

            _uiManager.UpdateInventory();
        }
    }
}