using UnityEngine;

namespace Terrain
{
    public class HexGridUtility
    {
        private static float _sqrt3 = Mathf.Sqrt(3);
        public static Vector3 GetPositionForHexFromCoordinate(int column, int row, float radius = 1f, bool isFlatTopped = false)
        {
            float width, height,  xPosition, yPosition, horizontalDistance, verticalDistance, offset;
            bool shouldOffset;
            float size = radius;

            if (!isFlatTopped)
            {
                shouldOffset = (row % 2) == 0;
                width = _sqrt3 * size;
                height = 2f * size;

                horizontalDistance = width;
                verticalDistance = height * (3f / 4f);

                offset = shouldOffset ? width / 2 : 0;

                xPosition = (column * horizontalDistance) + offset;
                yPosition = row * verticalDistance;
            }
            else
            {
                shouldOffset = (column % 2) == 0;
                width = 2f * size;
                height = _sqrt3 * size;

                horizontalDistance = width * (3f / 4f);
                verticalDistance = height;

                offset = shouldOffset ? height / 2 : 0;

                xPosition = column * horizontalDistance;
                yPosition = (row * verticalDistance) - offset;
            }

            return new Vector3(xPosition, 0, -yPosition);
        }

        public static Vector3Int OffsetToCube(Vector2Int offset, bool even = true)
        {
            var q = offset.x - (offset.y + (offset.y % 2)) / 2;
            
            if (!even && offset.y % 2 == 0)
                q--;
            
            var r = offset.y;
            return new Vector3Int(q, r, -q-r);
        }
    }
}