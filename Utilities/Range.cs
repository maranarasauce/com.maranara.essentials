using System;
using UnityEngine;

namespace Maranara.Utility
{
    [Serializable]
    public struct Range
    {
        public float Minimum;
        public float Maximum;
        public float Random()
        {
            return UnityEngine.Random.Range(Minimum, Maximum);
        }
    }

}
