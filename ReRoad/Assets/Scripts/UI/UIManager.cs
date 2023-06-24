using Core;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        private GameState _currentGameState;
        private GameState _savedGameState = GameState.WaitingPlayer;

        [Header("Player")]
        [SerializeField] private Player.Player _player;

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
        }

        private void Update()
        {
            _currentGameState = GameStateManager.GetState();

            // Setup not finished
            if (_currentGameState < GameState.WaitingPlayer)
                return;

            // If an action just ended, update needed
            if (_currentGameState == GameState.WaitingPlayer && _savedGameState != _currentGameState)
            {
                // Hide preview
                _actionPointUI.ShowActionPointUsedByMovement(false, 0);

                // Update actionPoint if the action used some
                _actionPointUI.UpdateActionPoint(_player.GetUsedActionPoint());

                // Show every possible action
                UpdateActionBar();

                // Update SurvivalStatus changed by the action
                _survivalUI.UpdateSurvivalStatus(_player.GetSurvivalStatus());

                // Update Inventories
                _inventoryUI.UpdateInventory();
            }

            // Player selecting the tile to move on
            if (_currentGameState == GameState.SelectingMove)
            {
                // Hide all possible action
                _actionBarUI.DeactivateAll();

                // Show the actionPoint that would be used
                _actionPointUI.ShowActionPointUsedByMovement(true, _player.move.GetActionPointPreview());
            }

            // Player currently moving
            if (_currentGameState == GameState.Moving)
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

            // Player asked to build, we propose it
            if (_currentGameState == GameState.Build && _savedGameState != _currentGameState)
            {
                _buildingUI.ProposeToBuild();

                // Hide all possible action
                _actionBarUI.DeactivateAll();
            }

            _savedGameState = _currentGameState;
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