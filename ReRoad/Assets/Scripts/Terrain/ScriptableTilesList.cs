using System;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain
{
    [CreateAssetMenu(menuName ="Terrain/Tile/List")]
    public class ScriptableTilesList : ScriptableObject
    {

        [Serializable]
        public class SerializableTileList
        {
            public TileType type;
            public GameObject tile;
        }

        [SerializeField] private List<SerializableTileList> _tiles;

        public GameObject GetTile(TileType tileType)
        {
            foreach(SerializableTileList tile in _tiles)
            {
                if (tile.type == tileType)
                    return tile.tile;
            }
            return null;
        }

        public GameObject GetRandomTile()
        {
            int index = UnityEngine.Random.Range(0, _tiles.Count);
            return _tiles[index].tile;
        }
    }
}