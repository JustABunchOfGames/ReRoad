using UnityEngine;
using Terrain;
using Resources;
using Core;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private bool _isReady = false;

        [Header("Position")]
        public Vector3Int cubeCoordinate;
        public Tile currentTile;

        [Header("Action Point")]
        [SerializeField] private int _maxActionPoint;
        [SerializeField] private int _actionPoint;

        [Header("SurvivalStatus")]
        [SerializeField] private SurvivalStatus _survivalStatus;

        [Header("Movement")]
        [SerializeField] private int _movePoint;
        private LineRenderer _lineRenderer;
        public PlayerMove move;

        [Header("Inventory")]
        [SerializeField] private int _inventoryMaxSize;
        public Inventory inventory;

        public void PlayerSetup(Tile startingTile)
        {
            // Setup Position
            cubeCoordinate = startingTile.cubeCoordinate;
            transform.position = startingTile.transform.position + new Vector3(0, transform.position.y, 0);
            currentTile = startingTile;

            // Setup actionPoints
            _actionPoint = _maxActionPoint;            

            // Reveal Fog of War
            startingTile.RevealTileAndNeighbours();

            // Setup Movement
            _lineRenderer = GetComponent<LineRenderer>();
            move = new PlayerMove(_lineRenderer, _movePoint, this);

            // Setup Inventory
            inventory = new Inventory(_inventoryMaxSize);
            inventory.Reveal();

            // We finished Setup
            _isReady = true;
        }

        public bool IsReady()
        {
            return _isReady;
        }

        public void UseActionPoint()
        {
            _actionPoint--;
            if (_actionPoint <= 0)
            {
                EndTurn();
                _actionPoint = _maxActionPoint;
            }
        }

        private void EndTurn()
        {
            _survivalStatus.stamina--;
            if (_survivalStatus.stamina <= 0)
            {
                _survivalStatus.stamina = 0;
                LooseHealth();
            }

            _survivalStatus.food--;
            if (_survivalStatus.food <= 0)
            {
                _survivalStatus.food = 0;
                LooseHealth();
            }

            _survivalStatus.water--;
            if(_survivalStatus.water <= 0)
            {
                _survivalStatus.water = 0;
                LooseHealth();
            }
        }

        private void LooseHealth()
        {
            _survivalStatus.health--;
            if (_survivalStatus.health <= 0)
            {
                _survivalStatus.health = 0;
                // GameOver
            }
        }

        public int GetUsedActionPoint()
        {
            return _maxActionPoint - _actionPoint;
        }

        public SurvivalStatus GetSurvivalStatus()
        {
            return _survivalStatus;
        }
    }
}