using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapMarker : MonoBehaviour
{
    [SerializeField] private Transform markerTransform;
    [SerializeField] private Tile markerTile;
    public Tile MarkerTile => markerTile;

    public bool isActive 
    {
        private get { return isActive; }
        set
        {
            if (value)
            {
                if (_activeMarkers.Contains(this) == false) _activeMarkers.Add(this);
                return;
            }
            _activeMarkers.Remove(this);
        }
    }
    private static List<MapMarker> _activeMarkers = new List<MapMarker>();
    public static List<MapMarker> ActiveMarkers => _activeMarkers;


    private void Start() { isActive = true; }

    public Vector2 GetPosition() { return markerTransform.position; }

    public void SetMarker(bool newActive) { isActive = newActive; }
}
