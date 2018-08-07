using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using NUnit.Framework.Internal.Execution;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{

	public List<Vector3> Points;
	private int destPoint = 0;
	private bool start = true;
	private int speed = 1;

	private void GotoNextPoint()
	{
		transform.position = Vector3.MoveTowards(transform.position, Points[destPoint], 0.02f);
		destPoint = (destPoint + 1) % Points.Count;
	}

	private void GotoPoint()
	{
		transform.position = Vector3.MoveTowards(transform.position, Points[destPoint], 0.02f);
	}

	void Update()
	{
		if (Points.Count == 0) return;
		if (Vector3.Distance(transform.position, Points[destPoint]) < 0.5f) GotoNextPoint();
		GotoPoint();
	}

}

