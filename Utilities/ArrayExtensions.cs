using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ArrayExtensions
{
    /// <summary>
    /// Pick a random element within the given array
    /// </summary>
    public static T Random<T>(this T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

    /// <summary>
    /// Check if layer is on within given LayerMask
    /// </summary>
    public static bool HasLayer(this LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }
}
