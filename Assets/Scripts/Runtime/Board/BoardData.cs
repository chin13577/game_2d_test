using FS.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using static FS.Util.Bounds2D;

namespace FS
{
    public class BoardData
    {
        // (row,col)
        // (0,0) (0,1) (0,2)
        // (1,0) (1,1) (1,2)
        // (2,0) (2,1) (2,2)
        // (3,0) (3,1) (3,2)

        public SlotInfo[,] SlotArr { get; private set; }
        public static readonly Vector3 TileImgPivot = new Vector2(0.5f, 0.5f);
        public Bounds2D Bound { get => this._bound; }
        private Bounds2D _bound;
        public int BoardOffsetX { get; private set; }
        public int BoardOffsetY { get; private set; }

        public int BoardWidth { get => SlotArr.GetLength(0); }
        public int BoardHeight { get => SlotArr.GetLength(1); }

        //Dictionary<Vector3Int, BoardObject> LandDict = new Dictionary<Vector3Int, BoardObject>();
        public int TotalSize
        {
            get
            {
                if (SlotArr == null)
                    return 0;
                return SlotArr.Length;
            }
        }

        public void Init(int width, int height)
        {
            _bound = new Bounds2D(0, 0, width, height);
            AnchorBounds2D anchor = _bound.Anchor;
            this.BoardOffsetX = Convert.ToInt32(anchor.centerLeft.x - _bound.Center.x);
            this.BoardOffsetY = Convert.ToInt32(anchor.centerTop.y - _bound.Center.y);

            InitSlotList(width, height);
        }

        public void ClearAll()
        {
            for (int i = 0; i < SlotArr.GetLength(0); i++)
            {
                for (int j = 0; j < SlotArr.GetLength(1); j++)
                {
                    if (SlotArr[i, j].Obj != null)
                    {
                        SlotArr[i, j].Obj.gameObject.SetActive(false);
                    }
                    SlotArr[i, j].Clear();
                }
            }
        }

        private void InitSlotList(int width, int height)
        {
            SlotArr = new SlotInfo[width, height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    SlotArr[i, j] = new SlotInfo(j, i, GetCenterTilePosition(j, i));
                }
            }
        }

        public bool IsOutOfRange(int col, int row)
        {
            if (col < 0 || col >= _bound.w)
                return true;
            else if (row < 0 || row >= _bound.h)
                return true;
            return false;
        }

        public bool IsOutOfRange(int col, int row, int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (IsOutOfRange(col + j, row + i) == true)
                        return true;
                }
            }
            return false;
        }

        public SlotInfo GetSlot(int col, int row)
        {
            if (IsOutOfRange(col, row))
                return null;

            return SlotArr[row, col];
        }

        public SlotInfo GetSlotFromPosition(Vector3 position)
        {
            Vector3Int arrCoordinate = ConvertWorldPosToArrayPos(position);
            if (IsOutOfRange(arrCoordinate.x, arrCoordinate.y))
                return null;

            int row = arrCoordinate.y;
            int col = arrCoordinate.x;
            return SlotArr[row, col];
        }

        public Vector3 GetCenterTilePosition(int col, int row)
        {
            // I've to -y because anchor of [0,0] is start at TopLeft.
            int posX = col + this.BoardOffsetX;
            int posY = -row + this.BoardOffsetY;
            return new Vector3(posX, posY) + TileImgPivot;
        }

        public Vector3Int ConvertWorldPosToArrayPos(Vector3 position)
        {
            Vector3Int posInt = position.ToVector3Int();
            int col = posInt.x - this.BoardOffsetX;
            int row = -posInt.y + this.BoardOffsetY;
            return new Vector3Int(col, row);
        }

        public Vector3Int ConvertArrayPosToWorldPos(int col, int row)
        {
            int posX = col + BoardOffsetX;
            int posY = -row + BoardOffsetY;
            return new Vector3Int(posX, posY);
        }

        public void SetObjectToSlot(int col, int row, ISlotInfo obj)
        {
            if (IsOutOfRange(col, row))
                return;
            SlotArr[row, col].SetObject(obj);
        }

        public List<SlotInfo> GetAllEmptySlots()
        {
            List<SlotInfo> result = new List<SlotInfo>();
            for (int i = 0; i < SlotArr.GetLength(0); i++)
            {
                for (int j = 0; j < SlotArr.GetLength(1); j++)
                {
                    if (SlotArr[i, j].IsEmpty)
                        result.Add(SlotArr[i, j]);
                }
            }
            return result;
        }

        public List<int> GetAllFullObstacleInCol()
        {
            List<int> result = new List<int>();
            for (int col = 0; col < SlotArr.GetLength(1); col++)
            {
                bool isHaveAllObstacle = true;
                for (int row = 0; row < SlotArr.GetLength(0); row++)
                {
                    if (SlotArr[row, col].IsObstacle == false)
                    {
                        isHaveAllObstacle = false;
                        break;
                    }
                }
                if (isHaveAllObstacle)
                    result.Add(col);
            }
            return result;
        }

        public List<int> GetAllFullObstacleHorizontal()
        {
            List<int> result = new List<int>();
            for (int row = 0; row < SlotArr.GetLength(0); row++)
            {
                bool isHaveAllObstacle = true;
                for (int col = 0; col < SlotArr.GetLength(1); col++)
                {
                    if (SlotArr[row, col].IsObstacle == false)
                    {
                        isHaveAllObstacle = false;
                        break;
                    }
                }
                if (isHaveAllObstacle)
                    result.Add(row);
            }
            return result;
        }

        public List<Bounds2D> GetAllEmptyBound(int width, int height)
        {
            List<Bounds2D> result = new List<Bounds2D>();
            for (int i = 0; i < SlotArr.GetLength(0); i++)
            {
                for (int j = 0; j < SlotArr.GetLength(1); j++)
                {
                    if (IsAvailableBound(j, i, width, height) == true)
                    {
                        result.Add(new Bounds2D(j, i, width, height));
                    }
                }
            }
            return result;
        }
        public bool IsAvailableBound(Bounds2D bound)
        {
            return IsAvailableBound((int)bound.x, (int)bound.y, (int)bound.w, (int)bound.h);
        }
        public bool IsAvailableBound(int col, int row, int width, int height)
        {
            if (IsOutOfRange(col, row, width, height) == true)
                return false;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (SlotArr[row + i, col + j].IsEmpty == false)
                        return false;
                }
            }
            return true;
        }
    }
}