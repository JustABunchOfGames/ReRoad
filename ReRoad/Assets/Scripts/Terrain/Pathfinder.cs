
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Terrain
{
    public class Pathfinder
    {
        public static List<Tile> FindPath(Tile origin, Tile destination)
        {
            Dictionary<Tile, Node> nodesNotEvaluated = new Dictionary<Tile, Node>();
            Dictionary<Tile, Node> nodesAlreadyEvaluated = new Dictionary<Tile, Node>();

            Node startNode = new Node(origin, origin, destination, 0);
            nodesNotEvaluated.Add(origin, startNode);

            bool gotPath = EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination, out List<Tile> path);

            while (!gotPath)
            {
                gotPath = EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination, out path);
            }
            
            return path;
        }

        private static bool EvaluateNextNode(Dictionary<Tile, Node> nodesNotEvaluated, Dictionary<Tile, Node> nodesEvaluated, Tile origin, Tile destination, out List<Tile> path)
        {
            Node currentNode = GetCheapestNode(nodesNotEvaluated.Values.ToArray());

            path = new List<Tile>();

            if (currentNode == null)
                return false;

            nodesNotEvaluated.Remove(currentNode.target);
            nodesEvaluated.Add(currentNode.target, currentNode);

            // End of the path
            if (currentNode.target == destination)
            {
                path.Add(currentNode.target);

                while(currentNode.target != origin)
                {
                    path.Add(currentNode.parent.target);
                    currentNode = currentNode.parent;
                }
                return true;
            }

            // Add neighbours node
            List<Node> neighbours = new List<Node>();
            foreach(Tile tile in currentNode.target.neighbours)
            {
                Node node = new Node(tile, origin, destination, currentNode.GetCost());

                neighbours.Add(node);
            }

            // Link all those neighbours
            foreach (Node neighbour in neighbours)
            {
                if (nodesEvaluated.Keys.Contains(neighbour.target))
                    continue;

                if (neighbour.GetCost() < currentNode.GetCost() || !nodesNotEvaluated.Keys.Contains(neighbour.target))
                {
                    neighbour.SetParent(currentNode);

                    if (!nodesNotEvaluated.Keys.Contains(neighbour.target))
                    {
                        nodesNotEvaluated.Add(neighbour.target, neighbour);
                    }
                }
            }

            return false;
        }

        private static Node GetCheapestNode(Node[] nodesNotEvaluated)
        {
            if (nodesNotEvaluated.Length == 0)
                return null;

            Node selectedNode = nodesNotEvaluated[0];
            Node currentNode;

            for (int i = 1; i < nodesNotEvaluated.Length; i++)
            {
                currentNode = nodesNotEvaluated[i];

                if (currentNode.GetCost() < selectedNode.GetCost())
                    selectedNode = currentNode;
                else if (currentNode.GetCost() == selectedNode.GetCost() && currentNode.GetCostToDestination() < selectedNode.GetCostToDestination())
                    selectedNode = currentNode;
            }

            return selectedNode;
        }
    }
}