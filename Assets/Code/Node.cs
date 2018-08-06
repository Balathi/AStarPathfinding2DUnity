using UnityEngine;

public class Node {

	public Vector3 GridPos;
	public Vector3 Pos;
	public bool isWall;

	public Node Parent;

	public int GCost;
	public int HCost;

	public Node(Vector3 gridPos, Vector3 pos, bool isWall) 
	{
		this.GridPos = gridPos;
		this.Pos = pos;
		this.isWall = isWall;
	}

	public int FCost() 
	{
		return GCost + HCost;
	}

}
