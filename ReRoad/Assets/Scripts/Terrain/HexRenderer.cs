using System.Collections.Generic;
using UnityEngine;

namespace Terrain
{
    // Face of an Hexagon
    public struct Face
    {
        public List<Vector3> vertices { get; }
        public List<int> triangles { get; }
        public List<Vector2> uvs { get; }

        public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
        {
            this.vertices = vertices;
            this.triangles = triangles;
            this.uvs = uvs;
        }
    }

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class HexRenderer : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;

        private List<Face> _faces;

        [Header("Size")]
        [SerializeField] private float _innerSize;
        [SerializeField] private float _outerSize;
        [SerializeField] private float _height;
        [SerializeField] private bool _isFlatTopped;

        [Space(10)]
        [Header("Material")]
        [SerializeField] private Material material;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();

            _mesh = new Mesh();

            _meshFilter.mesh = _mesh;
            _meshRenderer.material = material;
        }

        private void OnEnable()
        {
            DrawMesh();
        }

        private void OnValidate()
        {
            if (Application.isPlaying && _mesh != null)
            {
                DrawMesh();
            }
        }

        private void DrawMesh()
        {
            DrawFaces();
            CombineFaces();
        }

        private void DrawFaces()
        {
            _faces = new List<Face>();

            for (int point = 0; point < 6; point++)
            {
                _faces.Add(CreateFace(_innerSize, _outerSize, _height / 2, _height / 2, point));
            }
        }

        private Vector3 GetPoint(float size, float height, int index)
        {
            float angle_deg = _isFlatTopped ? 60 * index : 60 * index - 30;
            float angle_rad = Mathf.PI / 180 * angle_deg;
            return new Vector3(size * Mathf.Cos(angle_rad), height, size * Mathf.Sin(angle_rad));
        }
        
        private Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false)
        {
            Vector3 pointA = GetPoint(innerRad, heightB, point);
            Vector3 pointB = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
            Vector3 pointC = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
            Vector3 pointD = GetPoint(outerRad, heightA, point);

            List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
            List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
            List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

            if (reverse)
            {
                vertices.Reverse();
            }

            return new Face(vertices, triangles, uvs);
        }

        private void CombineFaces()
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            for (int i = 0; i < _faces.Count; i++)
            {
                // Add vertices
                vertices.AddRange(_faces[i].vertices);
                uvs.AddRange(_faces[i].uvs);

                // Offset and add triangles
                int offset = i * 4;
                foreach(int triangle in _faces[i].triangles)
                {
                    triangles.Add(triangle + offset);
                }
            }

            _mesh.vertices = vertices.ToArray();
            _mesh.triangles = triangles.ToArray();
            _mesh.uv = uvs.ToArray();
            _mesh.RecalculateNormals();
        }
    }
}