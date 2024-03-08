using UnityEngine;

namespace FS
{
    public class BoardObject
    {
        public GameObject Obj { get; set; }
        public IInteractable Interactable { get; private set; }
        public bool IsUsedArea { get => Obj != null; }
        public bool IsCanInteract { get => Interactable != null; }

        public void SetData(GameObject obj)
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