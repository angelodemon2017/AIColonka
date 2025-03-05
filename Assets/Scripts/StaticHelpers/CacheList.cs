using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CacheList<TKey, TElement> where TElement : ICachable<TKey>
{
    [SerializeField]
    private List<TElement> _elements = new List<TElement>();

    private Dictionary<TKey, TElement> _cache = new Dictionary<TKey, TElement>();

    public List<TElement> Elements => _elements;

    public TElement GetByKey(TKey key)
    {
        if (!_cache.TryGetValue(key, out var element))
        {
            element = _elements.Find(x => x.GetKey.Equals(key));
            if (element != null)
            {
                _cache[key] = element;
            }
            else
            {
                Debug.LogWarning($"Element with key {key} not found in CacheList.");
            }
        }
        return element;
    }

    public TElement this[int index]
    {
        get
        {
            if (index < 0 || index >= _elements.Count || _elements[index] == null)
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }
            return _elements[index];
        }
    }

    public void Add(TElement element)
    {
        if (element == null) return;

        _elements.Add(element);
        _cache[element.GetKey] = element;
    }

    public void Clear()
    {
        _elements.Clear();
        _cache.Clear();
    }

    public int Count()
    {
        return _elements.Count;
    }
}