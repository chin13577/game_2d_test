using UnityEngine;

namespace FS
{
    public interface IInteractable
    {
        /// <summary>
        /// Interact with the object
        /// </summary>
        /// <param name="user">represent to who are interact with this object</param>
        public void Interact(GameObject user);

    }
}