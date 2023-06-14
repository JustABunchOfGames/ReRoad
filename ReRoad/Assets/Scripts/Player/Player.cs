using UnityEngine;
using Core;
using Terrain;
using System.Collections;
using System.Collections.Generic;
using Resources;
using System.Runtime.CompilerServices;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [Header("Position")]
        public Vector3Int cubeCoordinate;
        public Tile currentTile;

        [Header("Action Point")]
        [SerializeField] private int _maxActionPoint;
        [SerializeField] private int _actionPoint;

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

            // Reveal Fog of War
            startingTile.RevealTileAndNeighbours();

            // Setup Movement
            _lineRenderer = GetComponent<LineRenderer>();
            move = new PlayerMove(_lineRenderer, this);

            // SetupInventory
            inventory = new Inventory(_inventoryMaxSize);
            inventory.Reveal();
        }
    }
}