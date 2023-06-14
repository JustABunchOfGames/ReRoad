using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Terrain
{
    public class MapChunkSpawner : MonoBehaviour
    {
        [SerializeField] private Vector2Int _mapSize;

        [SerializeField] private ScriptableChunksList _possibleChunks;
        [SerializeField] private int _chunkSize;
        [SerializeField] private float _radius;
        [SerializeField] private bool _isFlatTopped;

        public void Clear()
        {
            List<GameObject> chunks = new List<GameObject>();

            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject chunk = transform.GetChild(i).gameObject;
                chunks.Add(chunk);
            }

            foreach (GameObject chunk in chunks)
                DestroyImmediate(chunk);
        }

        public void LayoutGrid()
        {
            Clear();

            for (int y = 0; y < _mapSize.y; y++)
            {
                for (int x = 0; x < _mapSize.x; x++)
                {
                    GameObject chunk = Instantiate(_possibleChunks.GetRandomChunk());
                    chunk.name = $"Chunk {x},{y}";
                    chunk.transform.position = HexGridUtility.GetPositionForHexFromCoordinate(_chunkSize * x, _chunkSize * y, _radius, _isFlatTopped);
                    chunk.transform.SetParent(transform, true);

                    // Chunk coordinate into cube coordinate
                    Vector2Int chunkCornerCoordinate = new Vector2Int(_chunkSize * x, _chunkSize * y);
                    // (y%2==0 ? y : y+1)
                    foreach (Transform child in chunk.transform)
                    {
                        Tile tile = child.GetComponent<Tile>();
                        tile.offsetCoordinate += new Vector2Int(chunkCornerCoordinate.x, chunkCornerCoordinate.y);
                        tile.cubeCoordinate = HexGridUtility.OffsetToCube(tile.offsetCoordinate, y%2==0);

                        tile.gameObject.name = $"Hex {tile.offsetCoordinate.x},{tile.offsetCoordinate.y}";
                    }
                }
            }
        }

        [CustomEditor(typeof(MapChunkSpawner))]
        public class HexGridEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                MapChunkSpawner spawner = (MapChunkSpawner)target;

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Clear"))
                    spawner.Clear();
                if (GUILayout.Button("Layout"))
                    spawner.LayoutGrid();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}