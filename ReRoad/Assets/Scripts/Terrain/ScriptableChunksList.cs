using System.Collections.Generic;
using UnityEngine;

namespace Terrain
{
    [CreateAssetMenu(menuName = "Terrain/Chunk/List")]
    public class ScriptableChunksList : ScriptableObject
    {
        [SerializeField] private List<GameObject> _chunks;

        public GameObject GetRandomChunk()
        {
            return _chunks[Random.Range(0, _chunks.Count)];
        }
    }
}