using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public abstract Team Team { get; }

    public Direction CurrentDirection { get => _currentDirection; private set => _currentDirection = value; }
    private Direction _currentDirection = Direction.Right;

    public Direction LastDirection { get => _lastDirection; private set => _lastDirection = value; }
    private Direction _lastDirection = Direction.Right;

    public Vector3 LastPosition;
    public Vector3 CurrentPosition
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public void Move(Direction direction)
    {
        CurrentPosition += direction.ToVector3();
    }
}
