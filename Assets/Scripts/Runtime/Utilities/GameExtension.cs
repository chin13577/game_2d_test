using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameExtension
{
    public static List<T> Shuffle<T>(this List<T> list)
    {
        if (list.Count <= 1)
            return list;

        System.Random rnd = new System.Random();
        for (int i = 0; i < list.Count; i++)
        {
            int k = rnd.Next(0, i);
            T value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
        return list;
    }

    public static List<T> CutFirst<T>(this List<T> list, int amount)
    {
        List<T> result = new List<T>();
        for (int i = 0; i < list.Count; i++)
        {
            if (amount > 0)
            {
                result.Add(list[0]);
                list.RemoveAt(0);
                amount--;
                i--;
            }
            else
                break;
        }
        return result;
    }

    public static List<T> CutLast<T>(this List<T> list, int startIndex)
    {
        List<T> result = new List<T>();
        for (int i = startIndex; i < list.Count; i++)
        {
            result.Add(list[i]);
            list.RemoveAt(i);
            i--;
        }
        return result;
    }

    public static Vector3Int ToVector3Int(this Vector3 pos)
    {
        return new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
    }

    public static Vector3 ToVector3(this Vector3Int pos)
    {
        return new Vector3(pos.x, pos.y, pos.z);
    }

    public static Direction? ToDirection(this Vector2 vect)
    {
        if (vect.y == 1)
            return Direction.Up;
        else if (vect.y == -1)
            return Direction.Down;
        if (vect.x == 1)
            return Direction.Right;
        else if (vect.x == -1)
            return Direction.Left;

        return null;
    }

    public static Direction Inverse(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Right:
                return Direction.Left;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
        }
        return 0;
    }

    public static Vector3 ToVector3(this Direction direction)
    {
        Vector3 result = new Vector3();
        switch (direction)
        {
            case Direction.Up:
                result.y = 1;
                break;
            case Direction.Right:
                result.x = 1;
                break;
            case Direction.Down:
                result.y = -1;
                break;
            case Direction.Left:
                result.x = -1;
                break;
            default:
                break;
        }
        return result;
    }

    public static int ToInt32(this double value)
    {
        return System.Convert.ToInt32(value);
    }
    public static int ToInt32(this float value)
    {
        return System.Convert.ToInt32(value);
    }

}
