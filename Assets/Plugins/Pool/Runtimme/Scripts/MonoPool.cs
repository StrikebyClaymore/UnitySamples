using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class MonoPool<T> : IPool where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly List<T> _objects = new();

        public MonoPool(T prefab, int initialSize = 0, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            for (int i = 0; i < initialSize; i++)
            {
                Create();
            }
        }

        public T Get()
        {
            foreach (var obj in _objects)
            {
                if (!obj.gameObject.activeSelf)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
            var newObj = Create();
            newObj.gameObject.SetActive(true);
            return newObj;
        }

        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        private T Create()
        {
            var obj = Object.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(false);
            _objects.Add(obj);
            return obj;
        }
    }
}