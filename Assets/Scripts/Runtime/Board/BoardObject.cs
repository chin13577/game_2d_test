using UnityEngine;

namespace FS
{
    public class BoardObject
    {
        public int Col { get; private set; }
        public int Row { get; private set; }
        public Vector3 WorldPos { get; private set; }
        public GameObject Obj { get; set; }
        public IInteractable Interactable { get; private set; }
        public bool IsUsedArea { get => Obj != null; }
        public bool IsEmpty { get => Obj == null; }
        public bool IsCanInteract { get => Interactable != null; }

        public BoardObject(int col, int row, Vector3 worldPos)
        {
            this.Col = col;
            this.Row = row;
            this.WorldPos = worldPos;
        }

        public void SetObject(GameObject obj)
        {
            this.Obj = obj;
            Interactable = obj.GetComponent<IInteractable>();
        }

        public void Clear()
        {
            Obj = null;
            Interactable = null;
        }
    }
}