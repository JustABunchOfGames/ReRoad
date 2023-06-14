using UnityEngine;

namespace Terrain
{
    public class HexGridLayout : MonoBehaviour
    {
        [Header("GridSettings")]
        [SerializeField] private Vector2Int _gridSize;

        [Header("TileSettings")]
        [SerializeField] private float _outerSize;
        [SerializeField] private float _innerSize;
        [SerializeField] private float _height;
        [SerializeField] private bool _isFlatTopped;
        [SerializeField] private Material _material;

        private float sqrt3 = Mathf.Sqrt(3);

        private void OnEnable()
        {
            LayoutGrid();
        }

        private void LayoutGrid()
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                for (int x = 0; x < _gridSize.x; x++)
                {
                    GameObject tile = new GameObject($"Hex {x},{y}", typeof(HexRenderer));
                    tile.transform.position = GetPositionForHexFromCoordinate(x, y);

                    HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                    hexRenderer.SetInfo(_outerSize, _innerSize, _height, _isFlatTopped, _material);
                    hexRenderer.DrawMesh();

                    tile.transform.SetParent(transform, true);
                }
            }
        }

        private Vector3 GetPositionForHexFromCoordinate(int column, int row)
        {
            float width;
            float height;
            float xPosition;
            float yPosition;
            bool shouldOffset;
            float horizontalDistance;
            float verticalDistance;
            float offset;
            float size = _outerSize;

            if (!_isFlatTopped)
            {
                shouldOffset = (row % 2) == 0;
                width = sqrt3 * size;
                height = 2f * size;

                horizontalDistance = width;
                verticalDistance = height * (3f/4f);

                offset = shouldOffset ? width / 2 : 0;

                xPosition = (column * horizontalDistance) + offset;
                yPosition = row * verticalDistance;
            }
            else
            {
                shouldOffset = (column % 2) == 0;
                width = 2f * size;
                height = sqrt3 * size;

                horizontalDistance = width * (3f/4f);
                verticalDistance = height;

                offset = shouldOffset ? height / 2 : 0;

                xPosition = column * horizontalDistance;
                yPosition = (row * verticalDistance) - offset;
            }

            return new Vector3 (xPosition, 0, -yPosition);
        }
    }
}