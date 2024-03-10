using UnityEngine;

namespace FS
{
    public class SlotInfo
    {
        public int Col { get; private set; }
        public int Row { get; private set; }
        public Vector3 WorldPos { get; private set; }
        public ISlotInfo Obj { get; set; }
        public IInteractable Interactable { get; private set; }
        public bool IsHasObject { get => Obj != null; }
        public bool IsEmpty { get => Obj == null; }
        public bool IsCanInteract { get => Interactable != null; }
        public bool IsObstacle { get; private set; }

        public SlotInfo(int col, int row, Vector3 worldPos)
        {
            this.Col = col;
            this.Row = row;
            this.WorldPos = worldPos;
        }

        public void SetObject(ISlotInfo obj)
        {
            this.Obj = obj;
            Interactable = obj.gameObject.GetComponent<IInteractable>();
            IsObstacle = obj.gameObject.GetComponent<Obstacle>();
        }

        public void Clear()
        {
            Obj = null;
            Interactable = null;
            IsObstacle = false;
        }
    }
}