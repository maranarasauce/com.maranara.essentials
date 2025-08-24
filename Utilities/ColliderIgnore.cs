using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderIgnore : MonoBehaviour
{
    public bool includeSelfChildren = true;
    public Collider[] ignoredColliders;
    private void Awake()
    {
        Collider[] selfCols;
        if (includeSelfChildren)
        {
            selfCols = gameObject.GetComponentsInChildren<Collider>(true);
        }
        else
        {
            selfCols = gameObject.GetComponents<Collider>();
        }

        foreach (var a in selfCols)
        {
            foreach (var b in ignoredColliders)
            {
                Physics.IgnoreCollision(a, b);
            }
        }
    }
}
