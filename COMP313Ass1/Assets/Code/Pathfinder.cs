using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour {
    private Grid _grid;
    public Transform StartPos;
    public Transform EndPos;


    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    private void Update()
    {
        if (!StartPos.hasChanged && !EndPos.hasChanged) return;
        FindPath(StartPos.position, EndPos.position);
        StartPos.hasChanged = false;
        EndPos.hasChanged = false;
    }

    private void FindPath(Vector3 startPosPosition, Vector3 endPosPosition)
    {
        var startNode = _grid.GetNode(startPosPosition);
        var endNode = _grid.GetNode(endPosPosition);
        var nodes = new List<Node> {startNode};
        var checkedNodes = new HashSet<Node>();
        while (nodes.Count > 0)
        {
            //nodes.Sort((n1, n2)=> n1.fCost().CompareTo(n2.fCost()));
            var currentNode = FindBestNode(nodes);
            nodes.Remove(currentNode);
            checkedNodes.Add(currentNode);
            if (currentNode == endNode)
            {
                FindFinalPath(startNode, endNode);
                break;
            }
            foreach (var node in _grid.GetNighbours(currentNode))
            {
                if (node.isWall || checkedNodes.Contains(node)) continue;
                nodes.Add(SetCosts(node, currentNode, endNode));
            }
        }
    }


    private static Node FindBestNode(IList<Node> nodes)
    {
        var currentNode = nodes[0];
        for (var i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].FCost() < currentNode.FCost() ||
                nodes[i].FCost() == currentNode.FCost() && nodes[i].HCost < currentNode.HCost)
            {
                currentNode = nodes[i];
            }
        }
        return currentNode;
    }

    private Node SetCosts(Node node, Node parentNode, Node endNode)
    {
        node.GCost = parentNode.GCost + Distance(node, parentNode);
        node.HCost = Distance(node, endNode);
        node.Parent = parentNode;
        return node;
    }
    
    private static int Distance(Node to, Node from)
    {
        var dx = Mathf.Abs(to.GridPos.x - from.GridPos.x);
        var dy = Mathf.Abs(to.GridPos.y - from.GridPos.y);
        return Mathf.RoundToInt(dx + dy);
    }

    private void FindFinalPath(Node startNode, Node endNode)
    {
        var finalPath = new List<Node>();
        var currentNode = endNode;

        while (currentNode != startNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        finalPath.Reverse();
        _grid.FinalPath = finalPath;
    }
}
