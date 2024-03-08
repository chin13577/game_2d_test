using FS.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FS.Util.Bounds2D;

namespace FS
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private int _bgSize = 5;
        [SerializeField] private TilemapDrawer _tilemapDrawer;

        public Bounds2D Bound { get => this._bound; }
        private Bounds2D _bound;

        [Header("Debug")]
        [SerializeField] private int _debugX;
        [SerializeField] private int _debugY;

        public void SetBoardSize(int width, int height)
        {
            _bound = new Bounds2D(0, 0, width, height);

            _tilemapDrawer.ClearAllTiles();
            DrawMap(width + _bgSize, height + _bgSize, TilemapDrawer.TileType.WALL);
            DrawMap(width, height, TilemapDrawer.TileType.GROUND);
        }

        public void GenerateObstacle()
        {
            _tilemapDrawer.ClearObstacleTiles();

            //TODO: implement set obstacle.
            _tilemapDrawer.SetObstacle(new Vector3Int(4, 5));
            _tilemapDrawer.SetObstacle(new Vector3Int(4, 6));
            _tilemapDrawer.SetObstacle(new Vector3Int(5, 5));
            _tilemapDrawer.SetObstacle(new Vector3Int(5, 6));
        }

        [ContextMenu("TestDebugPosition")]
        public void DebugTilePosition()
        {
            //TODO: wait for delete;
            Vector3 pos = GetCenterTilePosition(this._debugX, this._debugY);
            Debug.Log(pos.x + " " + pos.y);
        }

        public Vector3 GetCenterTilePosition(int x, int y)
        {
            AnchorBounds2D anchor = _bound.Anchor;
            float spriteCenterPivot = 0.5f;
            int offsetX = Convert.ToInt32(anchor.centerLeft.x - _bound.Center.x);
            int offsetY = Convert.ToInt32(anchor.centerTop.y - _bound.Center.y);

            // I've to -y because anchor of [0,0] is start at TopLeft.
            return new Vector3(x + offsetX + spriteCenterPivot, -y + offsetY + spriteCenterPivot);
        }

        private void DrawMap(int width, int height, TilemapDrawer.TileType type)
        {
            Bounds2D bound = new Bounds2D(0, 0, width, height);
            AnchorBounds2D anchor = bound.Anchor;
            int offsetX = Convert.ToInt32(anchor.centerLeft.x - bound.Center.x);
            int offsetY = Convert.ToInt32(anchor.centerTop.y - bound.Center.y);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    // I've to -i because anchor of [0,0] is start at TopLeft.
                    _tilemapDrawer.SetBGTile(new Vector3Int(j + offsetX, -i + offsetY), type);
                }
            }
        }

    }
}