using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour {

	[SerializeField] Waypoint startWaypoint, endWaypoint;
	Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
	Queue<Waypoint> queue = new Queue<Waypoint>();
	bool isRunning = true;
	Waypoint searchCenter;

	Vector2Int[] directions =
	{
		Vector2Int.up,
		Vector2Int.right,
		Vector2Int.down,
		Vector2Int.left
	};

	// Use this for initialization
	void Start () {
        LoadBlocks();
        ColorStartAndEnd();
		Pathfind();
	}

    private void Pathfind()
    {
		queue.Enqueue(startWaypoint);

		while(queue.Count > 0 && isRunning)
        {
			searchCenter = queue.Dequeue();
			HaltIfEndFound();
			ExploreNeighbours();
			searchCenter.isExplored = true;
		}
		print("Finished pathfinding?");
	}

    private void HaltIfEndFound()
    {
		if (searchCenter == endWaypoint)
		{
			print("Searching from end node, therefore stopping"); // TODO  remove block
			isRunning = false;
		}
	}

    private void ExploreNeighbours()
    {
		if(!isRunning) { return; }
        foreach (Vector2Int direction in directions)
        {
			Vector2Int neighbourCoordinates = searchCenter.GetGridPos() + direction;
			try
            {
                QueueNewNeighbours(neighbourCoordinates);
            }
            catch
            {
				// do nothing
            }
		}
    }

    private void QueueNewNeighbours(Vector2Int neighbourCoordinates)
    {
        Waypoint neighbour = grid[neighbourCoordinates];
		if (neighbour.isExplored || queue.Contains(neighbour))
        {
			// do nothing..
		}
		else
        {
			
			queue.Enqueue(neighbour);
			neighbour.exploredFrom = searchCenter;
		}
    }

    private void ColorStartAndEnd()
    {
		startWaypoint.SetTopColor(Color.green);
		endWaypoint.SetTopColor(Color.red);
    }

    private void LoadBlocks()
    {
		var waypoints = FindObjectsOfType<Waypoint>();
		foreach (Waypoint waypoint in waypoints)
        {
			var gridPos = waypoint.GetGridPos();
			if (grid.ContainsKey(waypoint.GetGridPos()))
            {
				Debug.LogWarning("Skipping Overlapping block " + waypoint);
            }
			else
            {
				grid.Add(gridPos, waypoint);
			}
		}
		print(grid.Count);
    }
}
