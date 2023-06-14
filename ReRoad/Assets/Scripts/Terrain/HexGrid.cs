using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Terrain
{
    public class HexGrid : MonoBehaviour
    {
        [Header("GridSize")]
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private float _radius;
        [SerializeField] private bool _isFlatTopped;

        [Space(10)]
        [SerializeField] private ScriptableTilesList _possibleTiles;

        private float _sqrt3 = Mathf.Sqrt(3);

        public void Clear()
        {
            List<GameObject> tiles = new List<GameObject>();

            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject tile = transform.GetChild(i).gameObject;
                tiles.Add(tile);
            }

            foreach (GameObject tile in tiles)
                DestroyImmediate(tile);
        }

        public void LayoutGrid()
        {
            Clear();
            for (int y = 0; y < _gridSize.y; y++)
            {
                for (int x = 0;  x < _gridSize.x; x++)
                {
                    GameObject go = new GameObject($"Hex {x},{y}");
                    go.transform.parent = transform;
                    go.transform.position = HexGridUtility.GetPositionForHexFromCoordinate(x, y, _radius, _isFlatTopped);

                    Tile tile = go.AddComponent<Tile>();
                    tile.SetScriptable(_possibleTiles);
                    tile.RandomTileType();
                    tile.SetHexTile();

                    // Coordinates for navigation later on
                    tile.offsetCoordinate = new Vector2Int(x,y);
                    tile.cubeCoordinate = HexGridUtility.OffsetToCube(tile.offsetCoordinate);
                }
            }
        }

        [CustomEditor(typeof(HexGrid))]
        public class HexGridEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                HexGrid hexGrid = (HexGrid)target;

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Clear"))
                    hexGrid.Clear();
                if (GUILayout.Button("Layout"))
                    hexGrid.LayoutGrid();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}