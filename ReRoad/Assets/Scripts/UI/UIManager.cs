using Core;
using Player;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private PlayerData _player;

        [Header("UI")]
        [SerializeField] private SurvivalUI _survivalUI;
        [SerializeField] private ActionPointUI _actionPointUI;
        [SerializeField] private ActionBarUI _actionBarUI;
        [SerializeField] private InventoryUI _inventoryUI;
        [SerializeField] private BuildingUI _buildingUI;

        private IEnumerator Start()
        {
            // Waiting our turn by asking the manager every half second
            while (GameStateManager.GetState() != GameState.UISetup)
                yield return new WaitForSeconds(0.25f);

            // Setup every piece of UI
            _actionPointUI.UpdateActionPoint(_player.GetUsedActionPoint());

            UpdateActionBar();

            _survivalUI.UpdateSurvivalStatus(_player.GetSurvivalStatus());

            _inventoryUI.Setup(_player);
            _inventoryUI.UpdateInventory();

            _buildingUI.Setup(_player);

            GameStateManager.ChangeState();
        }

        public void SelectingMove(int actionPointUsed)
        {
            // Hide all possible action
            _actionBarUI.DeactivateAll();

            // Show the actionPoint that would be used
            _actionPointUI.ShowActionPointUsedByMovement(true, actionPointUsed);
        }

        public void CancelSelectingMove()
        {
            // Show every possible action
            UpdateActionBar();

            // Hide UI
            _actionPointUI.ShowActionPointUsedByMovement(false, 0);
        }

        public void Moving()
        {
            // Hide preview
            _actionPointUI.ShowActionPointUsedByMovement(false, 0);

            // Update actionPoint used during movement
            _actionPointUI.UpdateActionPoint(_player.GetUsedActionPoint());


            // Update SurvivalStatus changed by the movement
            _survivalUI.UpdateSurvivalStatus(_player.GetSurvivalStatus());

            // Update Inventories
            _inventoryUI.UpdateInventory();
        }

        public void LastMove()
        {
            Moving();

            // Show every possible action
            UpdateActionBar();
        }

        public void Harvest()
        {
            // Update actionPoint used during movement
            _actionPointUI.UpdateActionPoint(_player.GetUsedActionPoint());


            // Update SurvivalStatus changed by the movement
            _survivalUI.UpdateSurvivalStatus(_player.GetSurvivalStatus());

            // Update Inventories
            _inventoryUI.UpdateInventory();

            // Show every possible action
            UpdateActionBar();
        }

        public void UpdateInventory()
        {
            // Update Inventories
            _inventoryUI.UpdateInventory();
        }

        private void UpdateActionBar()
        {
            // Always show the move button
            _actionBarUI.ActivateStateButton(GameState.SelectingMove, true);

            // Don't activate the harvest button if the tile is already harvested
            _actionBarUI.ActivateStateButton(GameState.Harvest, !_player.currentTile.inventory.isRevealed());

            // If a building don't exist, propose to build
            _actionBarUI.ActivateStateButton(GameState.Build, !_player.currentTile.OutpostOnTile());
        }
    }
}