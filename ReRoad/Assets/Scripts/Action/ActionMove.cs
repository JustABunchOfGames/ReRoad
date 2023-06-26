using UnityEngine;
using System.Collections.Generic;
using Terrain;
using Player;
using Core;
using UI;
using System.Collections;

namespace Action
{
    public class ActionMove : MonoBehaviour
    {
        // PlayerData for position and ActionPoint
        PlayerData _player;

        // Calculating and showing ActionPoint that will be used
        private UIManager _uiManager;
        [SerializeField] private int _maxMovePoint;
        private int _actionPointPreview;

        // Camera for selecting the tile
        [SerializeField] private CameraSelectTile _camera;

        // Selecting and rendering the path
        private LineRenderer _lineRenderer;
        private List<Tile> _path;
        private Tile _savedTile;

        // HandleMovement variable
        private Tile _currentTile;
        private Tile _nextTile;
        private int _movePoint;

        public void Setup(PlayerData player, UIManager uiManager)
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _player = player;
            _uiManager = uiManager;
        }

        public void StartSelecting()
        {
            _savedTile = null;
            _camera.StartRaycast();
        }

        public void CancelSelecting()
        {
            // Stop raycasting
            _camera.StopRaycast();

            // Reset Renderer
            _path = new List<Tile>();
            RenderPath();

            // Hide ActionPoint preview
            _uiManager.CancelSelectingMove();
        }

        public void Selecting(Tile tile)
        {
            // Same Tile, no need to redo
            if (tile == _savedTile)
                return;

            // Calculating and rendering proposed path
            _path = Pathfinder.FindPath(_player.currentTile, tile);
            RenderPath();

            // Counting
            _movePoint = _maxMovePoint;

            // Save the actionPoint used for this movement, for UI
            _actionPointPreview = ((_path.Count - 1) / _maxMovePoint) + ((_path.Count - 1) %2);
            _uiManager.SelectingMove(_actionPointPreview);
        }

        private void RenderPath()
        {
            if (_lineRenderer == null)
                return;

            List<Vector3> points = new List<Vector3>();
            foreach (Tile tile in _path)
            {
                points.Add(tile.transform.position + new Vector3(0, _player.transform.position.y, 0));
            }
            _lineRenderer.positionCount = points.Count;
            _lineRenderer.SetPositions(points.ToArray());
        }

        public IEnumerator Move()
        {
            bool isMoving = HandleMovement();

            while (isMoving)
            {
                // Move & Use actionPoint accordingly
                yield return new WaitForSeconds(0.5f);
                isMoving = HandleMovement();
            }

            // Say to the manager we finished
            GameStateManager.ChangeState();
        }

        private bool HandleMovement()
        {
            // Handling Movement & returning a bool at false when the movement end

            // End of movement
            if (_path == null || _path.Count <= 1)
            {
                _nextTile = null;

                if (_path != null && _path.Count > 0)
                {
                    _currentTile = _path[0];
                    _nextTile = _currentTile;
                }

                RenderPath();

                // If movePoint are used partially at the end of the movement, still use an actionPoint
                if (_movePoint != _maxMovePoint)
                {
                    _player.UseActionPoint();
                }

                _uiManager.LastMove();
                return false;
            }

            // Normal movement
            _currentTile = _path[_path.Count - 1];

            _nextTile = _path[_path.Count - 2];

            _nextTile.RevealTileAndNeighbours();

            _player.transform.position = _nextTile.transform.position + new Vector3(0, _player.transform.position.y, 0);
            _player.currentTile = _nextTile;
            _player.cubeCoordinate = _nextTile.cubeCoordinate;

            // Using a movePoint (and an actionPoint if needed)
            UseMovePoint();

            // Update Path
            _path.RemoveAt(_path.Count - 1);
            RenderPath();

            _uiManager.Moving();
            return true;
        }

        private void UseMovePoint()
        {
            _movePoint--;
            if (_movePoint <= 0)
            {
                _movePoint = _maxMovePoint;
                _player.UseActionPoint();
            }
        }
    }
}