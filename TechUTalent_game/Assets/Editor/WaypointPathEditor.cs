using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaypointPath))]
public class WaypointPathEditor : Editor
{
    private WaypointPath _path;

    private void OnEnable()
    {
        _path = (WaypointPath)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Add Waypoint"))
        {
            AddWaypoint();
        }

        if (GUILayout.Button("Clear Waypoints"))
        {
            ClearWaypoints();
        }
    }

    private void OnSceneGUI()
    {
        for (int i = 0; i < _path.waypoints.Count; i++)
        {
            if (_path.waypoints[i] == null)
                continue;

            EditorGUI.BeginChangeCheck();

            Vector3 newPos = Handles.PositionHandle(
                _path.waypoints[i].position,
                Quaternion.identity
            );

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_path.waypoints[i], "Move Waypoint");
                _path.waypoints[i].position = newPos;
            }
        }
    }

    private void AddWaypoint()
    {
        GameObject wp = new GameObject($"Waypoint {_path.waypoints.Count}");
        Undo.RegisterCreatedObjectUndo(wp, "Add Waypoint");

        wp.transform.parent = _path.transform;

        if (_path.waypoints.Count > 0)
        {
            wp.transform.position =
                _path.waypoints[^1].position + Vector3.right;
        }
        else
        {
            wp.transform.position = _path.transform.position;
        }

        _path.waypoints.Add(wp.transform);
    }

    private void ClearWaypoints()
    {
        foreach (var wp in _path.waypoints)
        {
            if (wp != null)
                Undo.DestroyObjectImmediate(wp.gameObject);
        }

        _path.waypoints.Clear();
    }
}