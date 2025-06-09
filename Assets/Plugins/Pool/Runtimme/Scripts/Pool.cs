using System.Collections.Generic;

namespace Pool
{
    public class Pool<T> : IPool where T : IPoolObject, new()
    {
        private readonly List<T> _objects = new();

        public Pool(int initialSize = 0)
        {
            for (int i = 0; i < initialSize; i++)
            {
                Create();
            }
        }

        public T Get()
        {
            foreach (var obj in _objects)
            {
                if (obj.Active == false)
                {
                    obj.Active = true;
                    return obj;
                }
            }
            var newObj = Create();
            newObj.Active = true;
            return newObj;
        }

        public void Release(T obj)
        {
            obj.Active = false;
        }

        private T Create()
        {
            var obj = new T();
            _objects.Add(obj);
            return obj;
        }
    }
}