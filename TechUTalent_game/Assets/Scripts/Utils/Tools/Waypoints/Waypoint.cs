using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Waypoint System/Waypoint")]
public class Waypoint : MonoBehaviour
{
    [SerializeField] private string waypointName = "New Waypoint";
    public string WaypointName
    {
        get => waypointName;
        set => waypointName = value;
    }
    
    [SerializeField] private Color visualisationColor = Color.green;
    public Color VisualisationColor => visualisationColor;

    [SerializeField] private bool shouldDrawLines = true;
    public bool ShouldDrawLines
    {
        get => shouldDrawLines;
        set => shouldDrawLines = value;
    }
    
    [SerializeField] private bool shouldMoveWithParent = true;
    public bool ShouldMoveWithParent
    {
        get => shouldMoveWithParent;
        set => shouldMoveWithParent = value;
    }
    
    [SerializeField] private bool useParentAsStartPoint = true;
    public bool UseParentAsStartPoint
    {
        get => useParentAsStartPoint;
        set => useParentAsStartPoint = value;
    }
    
    [SerializeField] private List<Vector3> points = new List<Vector3>();
    public List<Vector3> Points
    {
        get => points;
        set => points = value;
    }
    
    public Vector3 this[Index i]
    {
        get => Points[i];
        set => Points[i] = value;
    }

    public int Length => Points.Count;

    public void Reverse() =>  Points.Reverse();
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = visualisationColor;
        if (Points == null || Points.Count == 0) return;

        for (int i = 0; i < Points.Count; i++)
        {
            var worldPos = transform.TransformPoint(Points[i]);
            Gizmos.DrawSphere(worldPos, 0.1f);

            if (shouldDrawLines && i < Points.Count - 1)
            {
                var nextWorld = transform.TransformPoint(Points[i + 1]);
                Gizmos.DrawLine(worldPos, nextWorld);
            }
        }

        // optionally draw line to parent start point
        if (useParentAsStartPoint && transform.parent != null && Points.Count > 0)
        {
            Gizmos.DrawLine(transform.parent.position, transform.TransformPoint(Points[0]));
        }
    }
#endif
}