using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDrawer : MonoBehaviour
{
    public enum TileType
    {
        GROUND,
        WALL,
        OBSTACLE
    }

    [Header("Tile")]
    [SerializeField] private TileBase _groundTile;
    [SerializeField] private TileBase _wallTile;
    [SerializeField] private TileBase _obstacleTile;

    [Header("Tilemap")]
    [SerializeField] private Tilemap _bgTilemap;
    [SerializeField] private Tilemap _obstacleTilemap;

    private Dictionary<Vector3Int, TileBase> _cacheBGTileDict = new Dictionary<Vector3Int, TileBase>();
    private Dictionary<Vector3Int, TileBase> _cacheObstacleTileDict = new Dictionary<Vector3Int, TileBase>();

    public void SetObstacle(Vector3Int position)
    {
        TileBase targetTile = GetTile(TileType.OBSTACLE);
        _obstacleTilemap.SetTile(position, targetTile);

        if (this._cacheObstacleTileDict.ContainsKey(position) == false)
            this._cacheObstacleTileDict.Add(position, null);
        this._cacheObstacleTileDict[position] = targetTile;
    }

    public void SetBGTile(Vector3Int position, TileType tileType)
    {
        TileBase targetTile = GetTile(tileType);
        _bgTilemap.SetTile(position, targetTile);
        SaveBGTileToCache(position, targetTile);
    }

    private void SaveBGTileToCache(Vector3Int position, TileBase targetTile)
    {
        if (this._cacheBGTileDict.ContainsKey(position) == false)
            this._cacheBGTileDict.Add(position, null);
        this._cacheBGTileDict[position] = targetTile;
    }

    private TileBase GetTile(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.WALL:
                return _wallTile;
            case TileType.GROUND:
                return _groundTile;
            case TileType.OBSTACLE:
            default:
                return _obstacleTile;
        }
    }

    public void ClearAllTiles()
    {
        ClearBGTiles();
        ClearObstacleTiles();
    }

    public void ClearObstacleTiles()
    {
        foreach (KeyValuePair<Vector3Int, TileBase> item in this._cacheObstacleTileDict)
        {
            _obstacleTilemap.SetTile(item.Key, null);
        }
        this._cacheObstacleTileDict.Clear();
    }

    public void ClearBGTiles()
    {
        foreach (KeyValuePair<Vector3Int, TileBase> item in this._cacheBGTileDict)
        {
            _bgTilemap.SetTile(item.Key, null);
        }
        this._cacheBGTileDict.Clear();
    }
}
