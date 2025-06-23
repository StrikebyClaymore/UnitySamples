using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISample.Utility
{
    [Serializable]
    public class PairCollection<TKey, TValue> : IEnumerable<Pair<TKey, TValue>>
    {
        [SerializeField] private List<Pair<TKey, TValue>> _items = new();
        public IReadOnlyList<Pair<TKey, TValue>> Items => _items;
        
        public TValue this[TKey key]
        {
            get
            {
                foreach (var pair in _items)
                {
                    if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
                        return pair.Value;
                }

                throw new KeyNotFoundException($"Key '{key}' not found.");
            }

            set
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (EqualityComparer<TKey>.Default.Equals(_items[i].Key, key))
                    {
                        _items[i] = new Pair<TKey, TValue>(key, value);
                        return;
                    }
                }

                _items.Add(new Pair<TKey, TValue>(key, value));
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var pair in _items)
            {
                if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
                {
                    value = pair.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            foreach (var pair in _items)
            {
                if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
                    return true;
            }
            return false;
        }

        public void Remove(TKey key)
        {
            _items.RemoveAll(p => EqualityComparer<TKey>.Default.Equals(p.Key, key));
        }

        public void Clear()
        {
            _items.Clear();
        }
        
        public IEnumerator<Pair<TKey, TValue>> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}