using UnityEngine;

namespace FS
{
    public interface IBoardObject
    {
        public Team Team { get; }
        public GameObject gameObject { get; }
    }
}