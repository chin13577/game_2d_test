using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public enum TileType
    {
        GROUND,
        WALL
    }

    public TileBase groundTile;
    public TileBase wallTile;
    public Tilemap tileMap;

    private Dictionary<Vector3Int, TileBase> _cacheTileDict = new Dictionary<Vector3Int, TileBase>();

    public void SetTile(Vector3Int position, TileType tileType)
    {
        TileBase targetTile = GetTile(tileType);
        tileMap.SetTile(position, targetTile);
        SaveTileToCache(position, targetTile);
    }

    private void SaveTileToCache(Vector3Int position, TileBase targetTile)
    {
        if (this._cacheTileDict.ContainsKey(position) == false)
            this._cacheTileDict.Add(position, null);
        this._cacheTileDict[position] = targetTile;
    }

    private void ClearAllTiles()
    {
        foreach (KeyValuePair<Vector3Int, TileBase> item in this._cacheTileDict)
        {
            tileMap.SetTile(item.Key, null);
        }
        this._cacheTileDict.Clear();
    }

    private TileBase GetTile(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.WALL:
                return wallTile;
            case TileType.GROUND:
            default:
                return groundTile;
        }
    }

}
