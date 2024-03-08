using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleData
{
    public int Width;
    public int Height;
    public int Size { get => Width * Height; }

    public ObstacleData(int width, int height)
    {
        this.Width = width;
        this.Height = height;
    }

}
