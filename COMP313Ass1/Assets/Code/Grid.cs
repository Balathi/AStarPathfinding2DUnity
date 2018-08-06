using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public Transform StartPos;
	public LayerMask WallMask;
	public Vector2 GridSizeWorld;
	public float NodeRadius;
	
	private Node[,] _grid;
	public List<Node> FinalPath;
	private float _nodeDiameter;

	private int _gridSizeX;
	private int _gridSizeY;

	private void Start() 
	{
		_nodeDiameter = NodeRadius * 2;
		_gridSizeX = Mathf.RoundToInt(GridSizeWorld.x / _nodeDiameter);
		_gridSizeY = Mathf.RoundToInt(GridSizeWorld.y / _nodeDiameter);
		SetupGrid();
	}

	private void SetupGrid() 
	{
		_grid = new Node[_gridSizeX, _gridSizeY];
		Vector3 topLeft = new Vector2(transform.position.x - GridSizeWorld.x / 2, transform.position.y - GridSizeWorld.y / 2);

		for (var x = 0; x < _gridSizeX; x++) {
			for (var y = 0; y < _gridSizeY; y++) {
				var pos = new Vector3 (topLeft.x + (_nodeDiameter * x), topLeft.y + (_nodeDiameter * y));
				var wall = Physics.CheckSphere (pos, NodeRadius, WallMask);
				_grid [x, y] = new Node (new Vector3 (x, y), pos, wall); 
			}
		}
	}

	private void OnDrawGizmos() 
	{
		Gizmos.DrawWireCube (transform.position, new Vector3 (GridSizeWorld.x, GridSizeWorld.y));
		if (_grid == null) return;
		foreach(var node in _grid) {
			Gizmos.color = node.isWall ? Color.red : Color.white;
			if (FinalPath != null)
			{
				if (FinalPath.Contains(node)) Gizmos.color = Color.green;
			}
			Gizmos.DrawWireCube (node.Pos, Vector3.one * (_nodeDiameter));
		}
	}

	public Node GetNode(Vector3 startPosPosition)
	{
		var xpoint = (startPosPosition.x + GridSizeWorld.x / 2) / GridSizeWorld.x;
		var ypoint = (startPosPosition.y + GridSizeWorld.y / 2) / GridSizeWorld.y;
		var x = Mathf.RoundToInt((_gridSizeX - 1) * xpoint);
		var y = Mathf.RoundToInt((_gridSizeY - 1) * ypoint);
		return _grid[x, y];
	}

	public List<Node> GetNighbours(Node node)
	{
		var nighbours = new List<Node>();
		var x = Mathf.RoundToInt(node.GridPos.x);
		var y = Mathf.RoundToInt(node.GridPos.y);
		if (InGridRange(x+1, y)) nighbours.Add(_grid[x+1, y]);
		if (InGridRange(x-1, y)) nighbours.Add(_grid[x-1, y]);
		if (InGridRange(x, y+1)) nighbours.Add(_grid[x, y+1]);
		if (InGridRange(x, y-1)) nighbours.Add(_grid[x, y-1]);
		return nighbours;
	}

	private bool InGridRange(int x, int y)
	{
		if (x < 0 || x >= _gridSizeX) return false;
		return y >= 0 && y < _gridSizeY;
	}
}
