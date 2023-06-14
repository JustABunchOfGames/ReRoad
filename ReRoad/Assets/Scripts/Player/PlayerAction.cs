using UnityEngine;
using Core;
using Terrain;
using System.Collections;

namespace Player
{
    public class PlayerAction : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private GameStateManager _gameStateManager;
        [SerializeField] private TileManager _tileManager;

        // Player
        private Player _player;

        // Move Action
        private Tile _savedTile;
        private bool _isMoving = false;

        // Update PlayerUI for movement and harvesting
        private PlayerUI _playerUI;

        private IEnumerator Start()
        {
            _player = GetComponent<Player>();
            _playerUI = GetComponent<PlayerUI>();

            // Waiting our turn by asking the manager every half second
            while (_gameStateManager.GetState() != GameState.PlayerSetup)
                yield return new WaitForSeconds(0.5f);

            // Setting up the player
            _player.PlayerSetup(_tileManager.GetRandom());

            _gameStateManager.ChangeState();

        }

        public void Update()
        {
            GameState state = _gameStateManager.GetState();

            // No state, no action
            if (state == GameState.WaitingPlayer)
                return;

            // Player did an action
            switch (state)
            {
                case GameState.Move:
                    StartCoroutine(Move());
                    break;
                case GameState.Harvest:
                    Harvest();
                    break;
                default:
                    break;
            }
        }

        private IEnumerator Move()
        {
            // Rendering path for the player to see where it will move
            if (_savedTile != _tileManager.GetHighlightedTile())
            {
                _savedTile = _tileManager.GetHighlightedTile();
                _player.move.SetDestination(_savedTile);
            }

            // Tile selected and confirmed, moving in progress
            if (_player.move != null && _tileManager.GetSelectedTile() != null && !_isMoving)
            {
                _isMoving = _player.move.HandleMovement();
                while (_isMoving)
                {
                    _playerUI.UpdateTileInventory();
                    yield return new WaitForSeconds(0.5f);
                    _isMoving = _player.move.HandleMovement();
                }

                // Reset all data used and needed
                _isMoving = false;
                _savedTile = null;

                // Say to the manager we finished
                _gameStateManager.ChangeState();
            }
        }

        private void Harvest()
        {
            _player.currentTile.inventory.Reveal();
            _playerUI.UpdateTileInventory();

            // Say to the manager we finished
            _gameStateManager.ChangeState();
        }
    }
}