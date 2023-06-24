using Core;
using Resources;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Terrain
{
    public class TileManager : MonoBehaviour
    {
        private Dictionary<Vector3Int, Tile> _tiles;

        [Header("HighlightingTile")]
        [SerializeField] private GameObject _highlight;
        [SerializeField] private GameObject _selector;

        private Tile _highlightedTile;
        private Tile _selectedTile;

        [Header("Manager")]
        [SerializeField] private GameStateManager _stateManager;

        [Header("RessourcesListForTiles")]
        [SerializeField] private ScriptableRessourcesByTileType _ressourcesList;
        [SerializeField] private int _baseInventorySizeForTiles = 20;

        [Header("FogOfWar")]
        [SerializeField] private GameObject _fogOfWarPrefab;

        private void Start()
        {
            _tiles = new Dictionary<Vector3Int, Tile>();

            Tile[] tileArray = GetComponentsInChildren<Tile>();

            // Register all tiles and give them all data necessary
            foreach (Tile tile in tileArray)
            {
                // Save Tiles
                RegisterTile(tile);

                // Add fog of war
                AddFogOfWarTile(tile);

                // Give them the ressources they need
                AddRessourcesToTile(tile);
            }

            // Set neighbours on each tiles
            foreach (Tile tile in tileArray)
            {
                tile.SetTileManager(this);
                List<Tile> list = GetNeighbours(tile);
                tile.neighbours = list;
            }

            // Say we have finished the initialization
            GameStateManager.ChangeState();
        }

        private void RegisterTile(Tile tile)
        {
            _tiles.Add(tile.cubeCoordinate, tile);
        }

        private void AddFogOfWarTile(Tile tile)
        {
            GameObject fow = Instantiate(_fogOfWarPrefab, transform);
            fow.name = "Fow " + tile.offsetCoordinate;
            fow.transform.position = tile.transform.position;
            tile.fow = fow;

            // Layer "Hidden", not visible on camera
            tile.SetLayer(6);
        }

        private void AddRessourcesToTile(Tile tile)
        {
            tile.inventory = new Inventory(_baseInventorySizeForTiles, _ressourcesList.GetRessourcesByTileType(tile.GetTileType()));
        }

        private List<Tile> GetNeighbours(Tile tile)
        {
            List<Tile> neighbours = new List<Tile>();

            Vector3Int[] neighbourCoords = new Vector3Int[]{
            new Vector3Int(1,-1,0),
            new Vector3Int(1,0,-1),
            new Vector3Int(0,1,-1),
            new Vector3Int(-1,1,0),
            new Vector3Int(-1,0,1),
            new Vector3Int(0,-1,1),
            };

            foreach(Vector3Int neighbourCoord in neighbourCoords)
            {
                Vector3Int tileCoord = tile.cubeCoordinate;

                if (_tiles.TryGetValue(tileCoord + neighbourCoord, out Tile neighbour))
                {
                    neighbours.Add(neighbour);
                }
            }
            return neighbours;
        }

        // ISelectable from Tile
        public void OnHighlightTile(Tile tile)
        {
            _selectedTile = null;

            _highlight.gameObject.SetActive(true);
            _highlight.transform.position = tile.transform.position;
            _highlightedTile = tile;
        }

        public Tile GetHighlightedTile()
        {
            return _highlightedTile;
        }

        public void OnSelectTile(Tile tile)
        {
            _highlight.gameObject.SetActive(false);
            _selector.gameObject.SetActive(true);
            _selector.transform.position = tile.transform.position;
            _selector.gameObject.SetActive(false);
            _selectedTile = tile;
        }

        public Tile GetSelectedTile()
        {
            return _selectedTile;
        }

        public void CancelSelectTile()
        {
            _highlightedTile = null;
            _highlight.gameObject.SetActive(false);
            _selector.gameObject.SetActive(false);
        }

        // For player to spawn on a random tile a the start of the game
        public Tile GetRandom()
        {
            return _tiles.ElementAt(Random.Range(0, _tiles.Count)).Value;
        }
    }
}