using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDrawer : MonoBehaviour
{
    [SerializeField] private Tilemap roomTilemap;
    [SerializeField] private Material tileMaterial;
    [SerializeField] private float tileSize = 0.1f;

    private Tilemap mapTilemap;
    private const string TILE_MAP_NAME = "Tiles";

    private void Start()
    {
        SetupTilemap();
        DrawMap();
    }

    private void SetupTilemap()
    {
        gameObject.AddComponent<Grid>();

        var child = new GameObject();
        child.transform.parent = transform;
        child.name = TILE_MAP_NAME;

        mapTilemap = child.AddComponent<Tilemap>();
    }

    public void DrawMap()
    {
        if (mapTilemap == null) return;

        var bounds = roomTilemap.cellBounds;
        var tiles = roomTilemap.GetTilesBlock(bounds);
        var positions = new List<Vector2>();

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase currentTile = tiles[x + y * bounds.size.x];
                if (currentTile == false) continue;

                positions.Add(new Vector2(x, y));
            }
        }

        // Add tile placing logic here
    }
}
