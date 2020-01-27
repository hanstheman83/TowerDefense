using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour {

	/// <summary>
	/// 
	/// </summary>
	[SerializeField] Waypoint startWaypoint, endWaypoint;

	Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
	Queue<Waypoint> queue = new Queue<Waypoint>();
	bool isRunning = true; // todo make private
	Waypoint searchCenter; // current searchCenter
	private List<Waypoint> path = new List<Waypoint>(); //  todo make private

	Vector2Int[] directions =
	{
		Vector2Int.up,
		Vector2Int.right,
		Vector2Int.down,
		Vector2Int.left
	};





	/// <summary>
	/// ///////////////////////////////////////////////////// METHODS ///////////////////////////////////////
	/// </summary>
	// Use this for initialization
	void Start () 
	{
		// 
	}

	public List<Waypoint> GetPath()
	{
		LoadBlocks();
		ColorStartAndEnd();
		BreadthFirstSearch();
		CreatePath();
		return path;
	}

	private void CreatePath()
	{
		path.Add(endWaypoint);
		Waypoint previous = endWaypoint.exploredFrom;
		while (previous != startWaypoint)
		{
			path.Add(previous);
			previous = previous.exploredFrom;
		}
		path.Add(startWaypoint);
		path.Reverse();
	}

	private void BreadthFirstSearch()
	{
		queue.Enqueue(startWaypoint);

		while(queue.Count > 0 && isRunning)
		{
			searchCenter = queue.Dequeue();
			//print("searching from : "+searchCenter);
			HaltIfEndFound();
			ExploreNeighbours();
			searchCenter.isExplored = true;
		}
	}

	private void HaltIfEndFound()
	{
		if(searchCenter == endWaypoint)
		{
			print("Searching from end node, therefore stopping");
			isRunning = false;
		}
	}

	private void ExploreNeighbours()
	{
		if(!isRunning) { return; }

		foreach (Vector2Int direction in directions)
		{
			Vector2Int neighbourCoordinates = searchCenter.GetGridPos() + direction;
			if(grid.ContainsKey(neighbourCoordinates))
			{
				QueueNewNeighbours(neighbourCoordinates);
			}
			else
			{
				// do nothing..
			}
		}
	}

	private void QueueNewNeighbours(Vector2Int neighbourCoordinates)
	{
		Waypoint neighbour = grid[neighbourCoordinates];
		if (neighbour.isExplored || queue.Contains(neighbour))
		{
			//do nothing
		}
		else 
		{
			queue.Enqueue(neighbour);
			neighbour.exploredFrom = searchCenter;
		}
	}

	private void ColorStartAndEnd()
	{
		startWaypoint.SetTopColor(Color.blue);
		endWaypoint.SetTopColor(Color.red);
	}

	private void LoadBlocks()
	{
		var waypoints = FindObjectsOfType<Waypoint>(); //find all objects in World with a Waypoint script on them..
		foreach (Waypoint waypoint in waypoints)
		{
			var gridPos = waypoint.GetGridPos();
			if(grid.ContainsKey(gridPos))
			{
				Debug.LogWarning("Overlapping block " + waypoint +" not added to dictionary");
			}
			else
			{
				grid.Add(waypoint.GetGridPos(), waypoint);
				//waypoint.SetTopColor(Color.blue);
			}
		}
		print("Loaded " + grid.Count + " blocks");
	}
}
