using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FS
{

    public class FlexiblePooling<T> where T : Component
    {
        private List<T> objects = new List<T>();
        private GameObject prefab;
        private Transform transform;

        public FlexiblePooling(Transform owner, T prefab, int initialAmount)
        {
            this.prefab = prefab.gameObject;
            this.transform = owner;
            InitialPrefabs(initialAmount);
        }

        private void InitialPrefabs(int initialAmount)
        {
            for (int i = 0; i < initialAmount; i++)
            {
                CreateAndAddToList();
            }
        }

        public void HideAllObject()
        {
            foreach (var item in objects)
            {
                item.gameObject.SetActive(false);
            }
        }

        public List<T> GetAllActiveObjects(Predicate<T> predicate)
        {
            return objects.FindAll(o => predicate(o) == true);
        }

        public T GetObject()
        {
            var obj = objects.Find(x => (x.gameObject.activeSelf == false));
            if (obj == null)
                obj = CreateAndAddToList();
            return obj;
        }

        private T CreateAndAddToList()
        {
            GameObject obj;
            if (transform == null)
            {
                obj = UnityEngine.Object.Instantiate(prefab);
            }
            else
            {
                obj = UnityEngine.Object.Instantiate(prefab, transform);
            }

            obj.SetActive(false);
            T component = obj.GetComponent<T>();
            objects.Add(component);
            return component;
        }
    }
}