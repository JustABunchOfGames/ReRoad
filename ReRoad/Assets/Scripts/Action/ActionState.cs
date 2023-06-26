using UnityEngine;
using System.Collections;
using Core;
using Player;
using Terrain;
using UI;

namespace Action
{
    public class ActionState : MonoBehaviour
    {
        // State
        private GameState _currentState;
        private GameState _savedGameState;

        [Header("Managers")]
        [SerializeField] private TileManager _tileManager;
        [SerializeField] private UIManager _uiManager;

        [Header("Player")]
        [SerializeField] private PlayerData _player;

        [Header("Actions")]
        [SerializeField] private ActionMove _actionMove;
        [SerializeField] private ActionHarvest _actionHarvest;
        [SerializeField] private ActionManageResource _actionManageResource;

        private IEnumerator Start()
        {
            // Waiting our turn by asking the manager every half second
            while (GameStateManager.GetState() != GameState.PlayerSetup)
                yield return new WaitForSeconds(0.25f);

            // Setting up the player
            _player.PlayerSetup(_tileManager.GetRandom());

            // Waiting the player setup
            while (!_player.IsReady())
                yield return new WaitForSeconds(0.25f);

            // Setting action
            _actionMove.Setup(_player, _uiManager);
            _actionHarvest.Setup(_player, _uiManager);
            _actionManageResource.Setup(_player, _uiManager);

            GameStateManager.ChangeState();
        }

        public void Update()
        {
            _currentState = GameStateManager.GetState();

            // state < Waiting, setup in progess
            if (_currentState < GameState.WaitingPlayer)
                return;

            // Player did an action
            if (_currentState != GameState.WaitingPlayer)
            {
                switch (_currentState)
                {

                    case GameState.SelectingMove:

                        // Only calling it once
                        if (_savedGameState != _currentState)
                            _actionMove.StartSelecting();

                        if (Input.GetButtonDown("Cancel"))
                        {
                            // Stop the selection on tileManager
                            _tileManager.StopSelectingTile();

                            // Stop the selection on actionMove
                            _actionMove.CancelSelecting();

                            // Reset State
                            GameStateManager.ChangeState();
                            break;
                        }

                        // Calling it again to see if the player selected another tile
                        _actionMove.Selecting(_tileManager.GetHighlightedTile());

                        // Tile selected and confirmed, moving in progress
                        if (_tileManager.GetSelectedTile() != null)
                        {
                            // Stop the selection because it's already done
                            _tileManager.StopSelectingTile();
                            GameStateManager.ChangeState(GameState.Moving);
                        }

                        break;

                    case GameState.Moving:

                        // Only calling it once
                        if (_savedGameState != _currentState)
                            StartCoroutine(_actionMove.Move());
                        break;

                    case GameState.Harvest:

                        // Only calling it once
                        if (_savedGameState != _currentState)
                            _actionHarvest.Harvest();
                        break;

                    default:
                        break;
                }
            }

            _savedGameState = _currentState;
        }

        public void Build(bool confirm)
        {
            if (confirm)
            {

            }

            // Say to the manager we finished
            GameStateManager.ChangeState();
        }
    }
}