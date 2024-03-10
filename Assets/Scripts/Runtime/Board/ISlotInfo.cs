using UnityEngine;

namespace FS
{
    public interface ISlotInfo
    {
        public Team Team { get; }
        public GameObject gameObject { get; }
    }
}