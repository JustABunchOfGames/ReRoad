using Core;
using Player;
using UI;
using UnityEngine;

namespace Action
{
    public class ActionHarvest : MonoBehaviour
    {
        // PlayerData for position and ActionPoint
        PlayerData _player;

        // Updating UI
        private UIManager _uiManager;

        public void Setup(PlayerData player, UIManager uiManager)
        {
            _player = player;
            _uiManager = uiManager;
        }

        public void Harvest()
        {
            // Reveal inventory for player
            _player.currentTile.inventory.Reveal();
            _uiManager.Harvest();

            // Update used actionPoint
            _player.UseActionPoint();

            // Say to the manager we finished
            GameStateManager.ChangeState();
        }
    }
}