using FS.Util;
using System;
using System.Collections;
using UnityEngine;
using static FS.Util.Bounds2D;

namespace FS
{

    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private int _bgSize = 5;

        public TilemapDrawer TilemapDrawer { get => _tilemapDrawer; }
        [SerializeField] private TilemapDrawer _tilemapDrawer;

        public Obstacle ObstaclePrefab { get => obstaclePrefab; }
        [SerializeField] private Obstacle obstaclePrefab;

        private ObstacleGenerator _obstacleGenerator;

        public BoardData BoardData { get => _boardData; }
        private BoardData _boardData = new BoardData();

        [Header("Debug")]
        [SerializeField] private int _debugX;
        [SerializeField] private int _debugY;


        public void Init()
        {
            _obstacleGenerator = new ObstacleGenerator(this);
        }

        public void SetBoardSize(int width, int height)
        {
            _boardData.Init(width, height);

            _tilemapDrawer.ClearAllTiles();
            _bgSize = width * 2;
            DrawMap(width + _bgSize, height + _bgSize, TilemapDrawer.TileType.WALL);
            DrawMap(width, height, TilemapDrawer.TileType.GROUND);
        }
        public void ClearData()
        {
            _boardData.ClearAll();
        }
        public void GenerateObstacle(float ratio)
        {
            _obstacleGenerator.HideAllObject();
            _tilemapDrawer.ClearObstacleTiles();
            _obstacleGenerator.GenerateObstacle(ratio, _boardData.TotalSize);
        }

        [ContextMenu("TestDebugPosition")]
        public void DebugTilePosition()
        {
            //TODO: wait for delete;
            Vector3 pos = _boardData.GetCenterTilePosition(this._debugX, this._debugY);
            Debug.Log(pos.x + " " + pos.y);
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
                    int x = j + offsetX;

                    // I've to -i because anchor of [0,0] is start at TopLeft.
                    int y = -i + offsetY;
                    _tilemapDrawer.SetBGTile(new Vector3Int(x, y), type);
                }
            }
        }

    }
}