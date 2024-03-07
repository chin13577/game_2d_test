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
          
        public void SetBoardSize(int width, int height)
        {
            _bound = new Bounds2D(0, 0, width, height);

            _tilemapDrawer.ClearAllTiles();
            DrawMap(width + _bgSize, height + _bgSize, TilemapDrawer.TileType.WALL);
            DrawMap(width, height, TilemapDrawer.TileType.GROUND);
        }

        private void DrawMap(int width, int height, TilemapDrawer.TileType type)
        {
            Bounds2D bound = new Bounds2D(0, 0, width, height);
            AnchorBounds2D anchor = bound.Anchor;
            int offsetX = System.Convert.ToInt32(anchor.centerLeft.x - bound.Center.x);
            int offsetY = System.Convert.ToInt32(anchor.centerBottom.y - bound.Center.y);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _tilemapDrawer.SetTile(new Vector3Int(j + offsetX, i + offsetY), type);
                }
            }
        }

    }
}