using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public bool loop;
    public bool showGizmos = true;

    public List<Transform> waypoints = new();

    public int Length => waypoints.Count;

    public Vector3 GetPoint(int index)
    {
        return waypoints[index].position;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos || waypoints.Count < 2)
            return;

        Gizmos.color = Color.cyan;

        for (var i = 0; i < waypoints.Count - 1; i++)
        {
            if (waypoints[i] == null || waypoints[i + 1] == null)
                continue;

            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }

        if (loop)
        {
            Gizmos.DrawLine(
                waypoints[^1].position,
                waypoints[0].position
            );
        }
    }
}