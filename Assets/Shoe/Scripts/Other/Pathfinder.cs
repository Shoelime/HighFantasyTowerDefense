using System.Linq;
using UnityEngine;

public class Pathfinder : IPathFinder
{
    private GameObject[] waypoints;
    public GameObject[] Waypoints => waypoints;

    private Transform entrancePoint;
    public Transform EntrancePoint => entrancePoint;

    public Pathfinder()
    {
        entrancePoint = GameObject.Find("EnemySpawnPosition").transform;

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint")
                        .OrderBy(wp => GetWaypointIndex(wp.name))
                        .ToArray();
    }

    private int GetWaypointIndex(string name)
    {
        // Extract the number from the waypoint name
        int startIndex = name.IndexOf('(') + 1;
        int endIndex = name.IndexOf(')');
        if (startIndex >= 0 && endIndex > startIndex)
        {
            string indexString = name.Substring(startIndex, endIndex - startIndex);
            if (int.TryParse(indexString, out int index))
            {
                return index;
            }
        }

        return 0;
    }
}