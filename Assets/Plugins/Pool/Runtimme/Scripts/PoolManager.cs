using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class PoolManager
    {
        private readonly List<IPool> _pools = new();
        
        public MonoPool<T> CreatePool<T>(T prefab, int initialSize = 0, Transform parent = null) where T : Component
        {
            var pool = new MonoPool<T>(prefab, initialSize, parent);
            return pool;
        }
        
        public bool TryGetPool<T>(out MonoPool<T> pool) where T : Component
        {
            pool = null;
            foreach (var p in _pools)
            {
                if (p is MonoPool<T> findPool)
                {
                    pool = findPool;
                    return true;
                }
            }
            return false;
        }
        
        public bool TryGetPool<T>(out Pool<T> pool) where T : IPoolObject, new()
        {
            pool = null;
            foreach (var p in _pools)
            {
                if (p is Pool<T> findPool)
                {
                    pool = findPool;
                    return true;
                }
            }
            return false;
        }
    }
}