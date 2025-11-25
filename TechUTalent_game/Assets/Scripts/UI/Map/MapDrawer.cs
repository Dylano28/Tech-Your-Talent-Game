using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDrawer : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    [SerializeField] private float baseSize = 0.1f;
    [SerializeField] private int orderInLayer = 50;
    [SerializeField] private string drawLayer = "UI";
    [SerializeField] private Tilemap roomTilemap;
    [SerializeField] private Tile mapTile;
    [SerializeField] private Tile playerTile;

    private Transform _mapTransform;
    private Tilemap _mapTilemap;
    private TilemapRenderer _renderer;

    private List<Vector3Int> _lastPlayerPositions = new List<Vector3Int>();

    private const string TILE_MAP_NAME = "Tiles";


    private void SetupTilemap()
    {
        gameObject.AddComponent<Grid>();

        var child = new GameObject();
        child.transform.parent = transform;
        child.name = TILE_MAP_NAME;

        _mapTransform = child.transform;
        _mapTilemap = child.AddComponent<Tilemap>();
        _renderer = child.AddComponent<TilemapRenderer>();

        _renderer.sortingOrder = orderInLayer;
        _renderer.sortingLayerName = drawLayer;
        _mapTransform.localScale = new Vector3(baseSize, baseSize, 0);
    }

    public void DrawMap()
    {
        if (_mapTilemap == null) SetupTilemap();
        _mapTilemap.ClearAllTiles();

        var bounds = roomTilemap.cellBounds;
        var tiles = roomTilemap.GetTilesBlock(bounds);
        var positions = new List<Vector3Int>();

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase currentTile = tiles[x + y * bounds.size.x];
                if (currentTile == false) continue;

                positions.Add(new Vector3Int(x, y));
            }
        }

        foreach (var pos in positions)
        {
            _mapTilemap.SetTile(pos + bounds.position, mapTile);
        }
        _mapTilemap.transform.localPosition = Vector2.zero; // Reset to parent position
    }

    public void DrawPlayers()
    {
        if (_mapTilemap == null) return;
        
        var players = GameObject.FindGameObjectsWithTag(playerTag);
        if (players.Length == 0) return;

        RemovePlayers();
        _lastPlayerPositions.Clear();

        foreach (var player in players)
        {
            var pos = player.transform.position;
            var tilePos = new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
            _lastPlayerPositions.Add(tilePos);

            _mapTilemap.SetTile(tilePos, playerTile);
        }
        _mapTilemap.transform.localPosition = Vector3.zero - ((Vector3)_lastPlayerPositions[0] * _mapTilemap.transform.localScale.x);
    }

    public void RemovePlayers()
    {
        if (_lastPlayerPositions.Count == 0) return;
        foreach (var pos in _lastPlayerPositions)
        {
            if (_mapTilemap.HasTile(pos) == false) continue;
            _mapTilemap.SetTile(pos, null);
        }
    }
}
