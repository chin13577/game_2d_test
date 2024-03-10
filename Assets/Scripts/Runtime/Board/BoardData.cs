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

        public BoardObject[,] BoardObjectArr { get; private set; }
        public static readonly Vector3 TileImgPivot = new Vector2(0.5f, 0.5f);
        public Bounds2D Bound { get => this._bound; }
        private Bounds2D _bound;
        public int BoardOffsetX { get; private set; }
        public int BoardOffsetY { get; private set; }

        //Dictionary<Vector3Int, BoardObject> LandDict = new Dictionary<Vector3Int, BoardObject>();
        public int TotalSize
        {
            get
            {
                if (BoardObjectArr == null)
                    return 0;
                return BoardObjectArr.Length;
            }
        }

        public void Init(int width, int height)
        {
            _bound = new Bounds2D(0, 0, width, height);
            AnchorBounds2D anchor = _bound.Anchor;
            this.BoardOffsetX = Convert.ToInt32(anchor.centerLeft.x - _bound.Center.x);
            this.BoardOffsetY = Convert.ToInt32(anchor.centerTop.y - _bound.Center.y);

            InitBoardObj(width, height);
        }

        public void ClearAll()
        {
            for (int i = 0; i < BoardObjectArr.GetLength(0); i++)
            {
                for (int j = 0; j < BoardObjectArr.GetLength(1); j++)
                {
                    BoardObjectArr[i, j].Clear();
                }
            }
        }

        private void InitBoardObj(int width, int height)
        {
            BoardObjectArr = new BoardObject[width, height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    BoardObjectArr[i, j] = new BoardObject(j, i, GetCenterTilePosition(j, i));
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

        public BoardObject GetBoardObject(int col, int row)
        {
            if (IsOutOfRange(col, row))
                return null;

            return BoardObjectArr[row, col];
        }

        public BoardObject GetBoardObjectFromPosition(Vector3 position)
        {
            Vector3Int arrCoordinate = ConvertWorldPosToArrayPos(position);
            if (IsOutOfRange(arrCoordinate.x, arrCoordinate.y))
                return null;

            int row = arrCoordinate.y;
            int col = arrCoordinate.x;
            return BoardObjectArr[row, col];
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

        public void SetObjectToBoard(int col, int row, IBoardObject obj)
        {
            if (IsOutOfRange(col, row))
                return;
            BoardObjectArr[row, col].SetObject(obj);
        }

        public List<BoardObject> GetAllEmptySlots()
        {
            List<BoardObject> result = new List<BoardObject>();
            for (int i = 0; i < BoardObjectArr.GetLength(0); i++)
            {
                for (int j = 0; j < BoardObjectArr.GetLength(1); j++)
                {
                    if (BoardObjectArr[i, j].IsEmpty)
                        result.Add(BoardObjectArr[i, j]);
                }
            }
            return result;
        }

        public List<Bounds2D> GetAllEmptyBound(int width, int height)
        {
            List<Bounds2D> result = new List<Bounds2D>();
            for (int i = 0; i < BoardObjectArr.GetLength(0); i++)
            {
                for (int j = 0; j < BoardObjectArr.GetLength(1); j++)
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
                    if (BoardObjectArr[row + i, col + j].IsEmpty == false)
                        return false;
                }
            }
            return true;
        }
    }
}