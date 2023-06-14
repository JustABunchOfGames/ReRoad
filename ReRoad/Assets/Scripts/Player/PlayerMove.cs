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
        private Player _player;

        // HandleMovement variable
        private Tile _currentTile;
        private Tile _nextTile;

        public PlayerMove(LineRenderer lineRenderer, Player player)
        {
            _lineRenderer = lineRenderer;
            _player = player;
        }

        public void SetDestination(Tile tile)
        {
            _path = Pathfinder.FindPath(_player.currentTile, tile);
            RenderPath();
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

        public bool HandleMovement()
        {
            if (_path == null || _path.Count <= 1)
            {
                _nextTile = null;

                if (_path != null && _path.Count > 0)
                {
                    _currentTile = _path[0];
                    _nextTile = _currentTile;
                }

                RenderPath();
                return false;
            }

            _currentTile = _path[_path.Count - 1];

            _nextTile = _path[_path.Count - 2];

            _nextTile.RevealTileAndNeighbours();

            _player.transform.position = _nextTile.transform.position + new Vector3(0, _player.transform.position.y, 0);
            _player.currentTile = _nextTile;
            _player.cubeCoordinate = _nextTile.cubeCoordinate;

            _path.RemoveAt(_path.Count - 1);
            RenderPath();
            return true;
        }
    }
}