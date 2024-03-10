using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{
    public abstract class Character : MonoBehaviour, IBoardObject
    {
        public Character Previous;
        public Character Next;

        public abstract Team Team { get; }

        public Direction NextDirection { get; set; }

        public Direction CurrentDirection { get; set; }
        public Direction LastDirection { get; set; }

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
            LastDirection = CurrentDirection;
            CurrentDirection = direction;
            CurrentPosition += direction.ToVector3();
        }
    }
}