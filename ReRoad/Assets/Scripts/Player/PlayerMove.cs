using UnityEngine;
using System.Collections.Generic;
using Terrain;

namespace Player
{
    public class PlayerMove
    {
        // Base variable
        private List<Tile> _path;
        private LineRenderer _lineRenderer;
        private int _maxMovePoint;
        private Player _player;

        // ActionPoint used in the previewed movement
        private int _actionPointPreview;

        // HandleMovement variable
        private Tile _currentTile;
        private Tile _nextTile;
        private int _movePoint;

        public PlayerMove(LineRenderer lineRenderer, int movePoint, Player player)
        {
            _lineRenderer = lineRenderer;
            _maxMovePoint = movePoint;
            _player = player;
        }

        public void SetDestination(Tile tile)
        {
            // Calculating and rendering proposed path
            _path = Pathfinder.FindPath(_player.currentTile, tile);
            RenderPath();

            // Counting
            _movePoint = _maxMovePoint;

            // Return the actionPoint that would be used for this movement
            _actionPointPreview = ((_path.Count - 1) / _maxMovePoint) + ((_path.Count - 1) %2);
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

        public int GetActionPointPreview()
        {
            return _actionPointPreview;
        }

        public bool HandleMovement()
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

        public void CancelMove()
        {
            _path = new List<Tile>();
            RenderPath();
        }
    }
}