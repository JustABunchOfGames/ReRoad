using System;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain
{
    [CreateAssetMenu(menuName = "Terrain/BuildByTileType")]
    public class ScriptableBuildByTileType : ScriptableObject
    {
        [Serializable]
        public class BuildByTileType
        {
            public TileType tileType;
            public GameObject buildPrefab;
        }

        [SerializeField] private List<BuildByTileType> _outpostList;

        public GameObject GetOutpostByTileType(TileType type)
        {
            foreach(BuildByTileType build in _outpostList)
            {
                if (build.tileType == type)
                    return build.buildPrefab;
            }
            return null;
        }
    }
}