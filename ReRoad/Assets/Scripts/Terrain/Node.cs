using Unity.VisualScripting.YamlDotNet.Serialization.NodeTypeResolvers;
using UnityEngine;

namespace Terrain
{
    public class Node
    {
        public Node parent;
        public Tile target;

        private Tile _origin;
        private Tile _destination;

        private int _baseCost;
        private int _costFromOrigin;
        private int _costToDestination;
        private int _pathCost;

        public Node(Tile current, Tile origin, Tile destination, int pathCost, int cost = 1)
        {
            parent = null;
            target = current;
            _origin = origin;
            _destination = destination;

            _baseCost = cost;

            _costFromOrigin = (int)Vector3Int.Distance(current.cubeCoordinate, origin.cubeCoordinate);
            _costToDestination = (int)Vector3Int.Distance(current.cubeCoordinate, destination.cubeCoordinate);

            _pathCost = pathCost;
        }

        public int GetCost()
        {
            return _pathCost + _baseCost + _costFromOrigin + _costToDestination;
        }

        public int GetCostToDestination()
        {
            return _costToDestination;
        }

        public void SetParent(Node node)
        {
            parent = node;
        }
    }
}