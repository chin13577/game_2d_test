using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS.Util
{

    public class RandomWeight<T>
    {
        private Dictionary<T, int> _itemDict = new Dictionary<T, int>();

        public void AddItem(T item, int weight)
        {
            if (_itemDict.ContainsKey(item) == false)
                _itemDict.Add(item, weight);
            else
                _itemDict[item] += weight;
        }

        public void Clear()
        {
            _itemDict.Clear();
        }

        public int TotalWeight
        {
            get
            {
                int result = 0;
                foreach (KeyValuePair<T, int> item in _itemDict)
                {
                    result += item.Value;
                }
                return result;
            }
        }

        public T Random()
        {
            int randomWeight = UnityEngine.Random.Range(0, TotalWeight);

            int currentWeight = 0;
            foreach (KeyValuePair<T,int> item in _itemDict)
            {
                currentWeight += item.Value;
                if (currentWeight > randomWeight)
                    return item.Key;
            }

            return default;
        }
    }

}