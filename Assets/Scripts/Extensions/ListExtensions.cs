using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListExtensions
{
    public static T GetRandom<T>(this IEnumerable<T> list)
    {
        return list.ElementAt(Random.Range(0, list.Count()));
    }

    public static T GetElement<T>(this IEnumerable<T> list, int index)
    {
        if (index >= list.Count())
        {
            index %= list.Count();
        }
        return list.ElementAt(index);
    }
}