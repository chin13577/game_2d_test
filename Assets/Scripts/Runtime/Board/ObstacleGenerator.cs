using FS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FS
{
    public class ObstacleGenerator
    {
        private BoardManager _boardManager;
        private BoardData _boardData;
        private TilemapDrawer _tilemapDrawer;
        private Dictionary<Size, float> generateObstacleRateConfig = new Dictionary<Size, float>();

        public FlexiblePooling<Obstacle> ObstaclePooling;

        public ObstacleGenerator(BoardManager boardManager)
        {
            this._boardManager = boardManager;
            this._boardData = boardManager.BoardData;
            this._tilemapDrawer = boardManager.TilemapDrawer;

            if (ObstaclePooling == null)
                ObstaclePooling = new FlexiblePooling<Obstacle>(null, boardManager.ObstaclePrefab, 1);
            ObstaclePooling.HideAllObject();
        }

        public void GenerateObstacle(float generateRatio, int totalBoardSize)
        {
            SpawnObstacle(generateRatio, totalBoardSize);
            RemoveDeadlockObstacleHorizontal();
            RemoveDeadlockObstacleVertical();
        }

        private void RemoveDeadlockObstacleVertical()
        {
            List<int> fullObstacleRowList = _boardData.GetAllFullObstacleHorizontal();
            for (int i = 0; i < fullObstacleRowList.Count; i++)
            {
                int randomDeleteColumn = UnityEngine.Random.Range(0, _boardData.BoardWidth);
                _tilemapDrawer.SetObstacle(_boardData.ConvertArrayPosToWorldPos(randomDeleteColumn, i));
                _boardData.GetSlot(randomDeleteColumn, i).Clear();
            }
        }

        private void RemoveDeadlockObstacleHorizontal()
        {
            List<int> fullObstacleRowList = _boardData.GetAllFullObstacleHorizontal();
            for (int i = 0; i < fullObstacleRowList.Count; i++)
            {
                int randomDeleteColumn = UnityEngine.Random.Range(0, _boardData.BoardWidth);
                _tilemapDrawer.SetObstacle(_boardData.ConvertArrayPosToWorldPos(randomDeleteColumn, i));
                _boardData.GetSlot(randomDeleteColumn, i).Clear();
            }
        }

        #region Spawn Obstacle

        private void SpawnObstacle(float generateRatio, int totalBoardSize)
        {
            generateRatio = Mathf.Clamp01(generateRatio);
            int totalObstacleSlot = Convert.ToInt32(totalBoardSize * generateRatio);

            Dictionary<Size, int> randomObstacleDict = RandomObstacle(totalObstacleSlot);
            foreach (KeyValuePair<Size, int> item in randomObstacleDict)
            {
                Size size = item.Key;
                List<Bounds2D> emptyList = _boardData.GetAllEmptyBound(size.width, size.height);
                emptyList.Shuffle();
                if (emptyList.Count == 0)
                    continue;

                int obstacleAmount = item.Value;
                while (obstacleAmount > 0)
                {
                    if (emptyList.Count == 0)
                        break;
                    Bounds2D bound = emptyList[0];
                    emptyList.RemoveAt(0);
                    if (_boardData.IsAvailableBound(bound) == false)
                    {
                        continue;
                    }

                    SpawnObstacle(bound.x, bound.y, bound.w, bound.h);
                    obstacleAmount -= 1;
                }
            }
        }

        private Dictionary<Size, int> RandomObstacle(int totalObstacleSlot)
        {
            Dictionary<Size, int> catagorizeObstacle = new Dictionary<Size, int>();
            catagorizeObstacle.Add(new Size(2, 2), 0);
            catagorizeObstacle.Add(new Size(2, 1), 0);
            catagorizeObstacle.Add(new Size(1, 2), 0);
            catagorizeObstacle.Add(new Size(1, 1), 0);

            List<Size> keys = catagorizeObstacle.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                Size size = keys[i];
                int maxSpawnCount = totalObstacleSlot / size.TotalSize;
                int randomSpawnCount = UnityEngine.Random.Range(0, maxSpawnCount);
                catagorizeObstacle[size] = randomSpawnCount;
                totalObstacleSlot -= (randomSpawnCount * size.TotalSize);
            }

            catagorizeObstacle[new Size(1, 1)] += totalObstacleSlot;

            return catagorizeObstacle;
        }

        private void SpawnObstacle(float col, float row, float width, float height)
        {
            SpawnObstacle((int)col, (int)row, (int)width, (int)height);
        }

        private void SpawnObstacle(int col, int row, int width, int height)
        {
            if (_boardData.IsOutOfRange(col, row, width, height))
                return;

            Obstacle obstacle = ObstaclePooling.GetObject();
            obstacle.gameObject.SetActive(true);
            obstacle.SetData(new ObstacleData(col, row, width, height));

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _boardData.SetObjectToSlot(col + j, row + i, obstacle);
                    _tilemapDrawer.SetObstacle(_boardData.ConvertArrayPosToWorldPos(col + j, row + i));
                }
            }
        }

        #endregion
        public void HideAllObject()
        {
            ObstaclePooling.HideAllObject();
        }
    }
}