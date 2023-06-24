using UnityEngine;
using Core;
using Terrain;
using System.Collections;

namespace Player
{
    public class PlayerState : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private TileManager _tileManager;

        // Player
        private Player _player;

        // State
        private GameState _state;

        // Move Action
        private Tile _savedTile;
        private bool _isMoving = false;

        private IEnumerator Start()
        {
            _player = GetComponent<Player>();

            // Waiting our turn by asking the manager every half second
            while (GameStateManager.GetState() != GameState.PlayerSetup)
                yield return new WaitForSeconds(0.25f);

            // Setting up the player
            _player.PlayerSetup(_tileManager.GetRandom());

            // Waiting the player setup
            while (!_player.IsReady())
                yield return new WaitForSeconds(0.25f);

            GameStateManager.ChangeState();
        }

        public void Update()
        {
            _state = GameStateManager.GetState();

            // state < Waiting, setup in progess
            if (_state < GameState.WaitingPlayer)
                return;

            // Player did an action
            if (_state != GameState.WaitingPlayer)
            {

                switch (_state)
                {
                    case GameState.SelectingMove:
                        SelectingMove();
                        break;

                    case GameState.Moving:
                        StartCoroutine(Move());
                        break;

                    case GameState.Harvest:
                        Harvest();
                        break;

                    default:
                        break;
                }
            }
            else
            {
                // We cancelled the movement
                if (_savedTile != null)
                    CancelMove();
            }
        }

        private void SelectingMove()
        {
            // Rendering path for the player to see where it will move
            if (_savedTile != _tileManager.GetHighlightedTile())
            {
                _savedTile = _tileManager.GetHighlightedTile();
                _player.move.SetDestination(_savedTile);
            }

            // Tile selected and confirmed, moving in progress
            if (_player.move != null && _tileManager.GetSelectedTile() != null && !_isMoving)
                GameStateManager.ChangeState(GameState.Moving);
        }

        private IEnumerator Move()
        {
            // Already called and moving
            if (_isMoving)
                yield break;

            _isMoving = _player.move.HandleMovement();
            while (_isMoving)
            {
                // Move & Use actionPoint accordingly
                yield return new WaitForSeconds(0.5f);
                _isMoving = _player.move.HandleMovement();
            }

            // Reset all data used and needed
            _isMoving = false;
            _savedTile = null;

            // Say to the manager we finished
            GameStateManager.ChangeState();
        }

        private void CancelMove()
        {
            // Cancel Highlight
            _tileManager.CancelSelectTile();

            // Cancel PathRendering
            _player.move.CancelMove();

            // Reset all data used and needed
            _isMoving = false;
            _savedTile = null;
        }

        private void Harvest()
        {
            // Reveal inventory for player
            _player.currentTile.inventory.Reveal();

            // Update used actionPoint
            _player.UseActionPoint();

            // Say to the manager we finished
            GameStateManager.ChangeState();
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