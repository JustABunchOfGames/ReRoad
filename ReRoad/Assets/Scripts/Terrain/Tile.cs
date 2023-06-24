using Build;
using Core;
using Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain
{
    public enum TileType
    {
        Plain,
        Forest,
        RockyPlain,
        AbandonnedVillage,
        Mountain,
        River,
        Sand, 
        COUNT
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [SelectionBase]
#endif
    public class Tile : MonoBehaviour, ISelectable
    {
        // List of all possible Tile
        private ScriptableTilesList _possibleTiles;

        // Type of this tile
        [SerializeField] private TileType _type;

        // Visible tile
        private GameObject _hexTile;

        [Header("NavigationCoordinate")]
        public Vector2Int offsetCoordinate;
        public Vector3Int cubeCoordinate;

        // TileManager (Set Neighbours and do selected things)
        private TileManager _tileManager;
        public List<Tile> neighbours;

        [Header("FogOfWar")]
        public GameObject fow;

        [Header("Inventory")]
        public Inventory inventory;

        private Outpost _outpost;

        public void SetTileManager(TileManager manager)
        {
            _tileManager = manager;
        }

        public TileType GetTileType()
        {
            return _type;
        }

        // For Hexgrid, wich spawn tiles
        public void SetScriptable(ScriptableTilesList tileList)
        {
            _possibleTiles = tileList;
        }

        public void RandomTileType()
        {
            _type = (TileType)Random.Range(0, (int)TileType.COUNT);
        }

        // Update _hexTile, the visible object for the tile
        public void SetHexTile()
        {
            _hexTile = Instantiate(_possibleTiles.GetTile(_type), transform);

            if (GetComponent<MeshCollider>() == null)
            {
                MeshCollider collider = gameObject.AddComponent<MeshCollider>();
                collider.sharedMesh = _hexTile.GetComponent<MeshFilter>().sharedMesh;
            }
        }

        // ISelectable
        public void OnHighlight()
        {
            _tileManager.OnHighlightTile(this);
        }

        public void OnSelect()
        {
            _tileManager.OnSelectTile(this);
        }

        public void RevealTileAndNeighbours()
        {
            Reveal();
            foreach (Tile neighbour in neighbours)
            {
                neighbour.Reveal();
            }
        }

        public void SetLayer(int layer)
        {
            gameObject.layer = layer;
            
            SetLayerInChildren(transform, layer);
        }

        private void SetLayerInChildren(Transform transform, int layer)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.layer = layer;
                SetLayerInChildren(child, layer);
            }
        }

        private void Reveal()
        {
            // Layer "Default"
            SetLayer(0);

            fow.gameObject.SetActive(false);
        }

        public bool IsRevealed()
        {
            return !fow.gameObject.activeSelf;
        }

        public bool OutpostOnTile()
        {
            return _outpost != null;
        }


#if UNITY_EDITOR
        // Change _hexTile automatically if we change the TileType in the editor
        private bool _isDirty = false;

        private void OnValidate()
        {
            if (_hexTile == null)
                return;
            _isDirty = true;
        }

        private void Update()
        {
            if (_isDirty)
            {
                if (Application.isPlaying)
                    Destroy(_hexTile);
                else
                    DestroyImmediate(_hexTile);
                SetHexTile();
                _isDirty = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (neighbours == null || neighbours.Count == 0)
                return;
            foreach (Tile neighbour in neighbours)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(transform.position, 0.1f);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(transform.position, neighbour.transform.position);
            }
        }
    }
#endif
}