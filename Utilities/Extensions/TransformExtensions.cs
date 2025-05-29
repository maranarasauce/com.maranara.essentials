using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Sets the local position and rotation to their identities
    /// </summary>
    public static void LocalReset(this Transform tsf)
    {
        tsf.localPosition = Vector3.zero;
        tsf.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Parents to the 'parent' and sets local pos/rot to identities
    /// </summary>
    public static void ParentReset(this Transform tsf, Transform parent)
    {
        tsf.SetParent(parent, true);
        tsf.LocalReset();
    }
}
