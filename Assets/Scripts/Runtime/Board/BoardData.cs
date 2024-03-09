﻿using FS.Util;
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
                    BoardObjectArr[i, j] = new BoardObject();
                }
            }
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

        public bool IsOutOfRange(int col, int row)
        {
            if (col < 0 || col >= _bound.w)
                return true;
            else if (row < 0 || row >= _bound.h)
                return true;
            return false;
        }

        public BoardObject GetBoardObjectFromPosition(int posX, int posY)
        {
            Vector3Int arrCoordinate = ConvertWorldPosToArrayPos(posX, posY);
            if (IsOutOfRange(arrCoordinate.x, arrCoordinate.y))
                return null;

            int row = arrCoordinate.y;
            int col = arrCoordinate.x;
            return BoardObjectArr[row, col];
        }

        public Vector3Int ConvertWorldPosToArrayPos(int posX, int posY)
        {
            int col = posX - this.BoardOffsetX;
            int row = -posY + this.BoardOffsetY;
            return new Vector3Int(col, row);
        }

        public Vector3Int ConvertArrayPosToWorldPos(int col, int row)
        {
            int posX = col + BoardOffsetX;
            int posY = -row + BoardOffsetY;
            return new Vector3Int(posX, posY);
        }

        public void SetObjectToBoard(int col, int row, GameObject obj)
        {
            if (IsOutOfRange(col, row))
                return;
            BoardObjectArr[row, col].SetData(obj);
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