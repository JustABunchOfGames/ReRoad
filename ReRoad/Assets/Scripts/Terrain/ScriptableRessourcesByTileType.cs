using UnityEngine;
using Resources;
using System.Collections.Generic;
using System;

namespace Terrain
{
    [CreateAssetMenu(menuName = "Terrain/RessourcesByTileType")]
    public class ScriptableRessourcesByTileType : ScriptableObject
    {
        [Serializable]
        public class InventoryForTileType
        {
            [Header("TileType")]
            public TileType type;

            [Header("GuaranteedRessources")]
            [SerializeField] private List<Resource> _guaranteedRessource;
            [SerializeField] private AnimationCurve _guaranteedRessourceRandomCurve;

            [Header("OptionnalRessources")]
            [SerializeField] private List<Resource> _optionnalRessource;
            [SerializeField] private AnimationCurve _optionnalRessourceRandomCurve;

            public List<Resource> GetRssources()
            {
                List<Resource> list = new List<Resource>();

                float random;

                // Get 1 Ressource from the guaranteed pool
                if (_guaranteedRessource.Count > 0)
                {
                    random = UnityEngine.Random.value * _guaranteedRessource.Count;
                    list.Add(_guaranteedRessource[(int)_guaranteedRessourceRandomCurve.Evaluate(random)]);
                }

                if (_optionnalRessource.Count > 0)
                {
                    // Get 1 Ressource from the optionnal pool
                    random = UnityEngine.Random.value * _optionnalRessource.Count;
                    list.Add(_optionnalRessource[(int)_optionnalRessourceRandomCurve.Evaluate(random)]);
                }

                return list;
            }
        }

        [SerializeField] private List<InventoryForTileType> _inventoryForTileTypeList;

        public List<Resource> GetRessourcesByTileType(TileType type)
        {
            List<Resource> list = new List<Resource>();

            foreach(InventoryForTileType inventory in _inventoryForTileTypeList)
            {
                if (inventory.type == type)
                {
                    list = inventory.GetRssources();
                }
            }
            return list;
        }
    }
}