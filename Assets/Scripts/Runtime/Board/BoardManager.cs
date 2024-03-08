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
        public static readonly Vector3 TileImgPivot = new Vector2(0.5f, 0.5f);
        [SerializeField] private int _bgSize = 5;
        [SerializeField] private TilemapDrawer _tilemapDrawer;

        [SerializeField] private Obstacle obstaclePrefab;
        public FlexiblePooling<Obstacle> obstaclePooling;

        private BoardData _boardData = new BoardData();

        [Header("Debug")]
        [SerializeField] private int _debugX;
        [SerializeField] private int _debugY;

        public void Init()
        {
            if (obstaclePooling == null)
                obstaclePooling = new FlexiblePooling<Obstacle>(null, obstaclePrefab, 1);
            obstaclePooling.HideAllObject();
        }

        public void SetBoardSize(int width, int height)
        {
            _boardData.Init(width, height);

            _tilemapDrawer.ClearAllTiles();
            DrawMap(width + _bgSize, height + _bgSize, TilemapDrawer.TileType.WALL);
            DrawMap(width, height, TilemapDrawer.TileType.GROUND);
        }

        public void GenerateObstacle(int amount)
        {
            obstaclePooling.HideAllObject();
            _tilemapDrawer.ClearObstacleTiles();

            RandomWeight<ObstacleData> randomWeight = new RandomWeight<ObstacleData>();
            randomWeight.AddItem(new ObstacleData(1, 1), 20);
            randomWeight.AddItem(new ObstacleData(1, 2), 10);
            randomWeight.AddItem(new ObstacleData(2, 1), 10);
            randomWeight.AddItem(new ObstacleData(2, 2), 5);

            for (int i = 0; i < amount; i++)
            {
                ObstacleData obstacleData = randomWeight.Random();

                //TODO: find the valid place to spawn obstacle.
                SpawnObstacle(UnityEngine.Random.Range(0, 16), UnityEngine.Random.Range(0, 16), obstacleData.Width, obstacleData.Height);
            }
        }

        private void SpawnObstacle(int col, int row, int width, int height)
        {
            if (_boardData.IsOutOfRange(col, row, width, height))
                return;

            Obstacle obstacle = obstaclePooling.GetObject();
            obstacle.gameObject.SetActive(true);
            obstacle.SetData(new ObstacleData(width, height));

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _boardData.SetObjectToBoard(col + j, row + i, obstacle.gameObject);
                    _tilemapDrawer.SetObstacle(_boardData.ConvertArrayPosToWorldPos(col + j, row + i));
                }
            }
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
            // I've to -y because anchor of [0,0] is start at TopLeft.
            int posX = x + _boardData.BoardOffsetX;
            int posY = -y + _boardData.BoardOffsetY;
            return new Vector3(posX, posY) + TileImgPivot;
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