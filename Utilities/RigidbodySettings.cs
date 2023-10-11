using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySettings : MonoBehaviour
{
    public Transform comOverride;
    Rigidbody selfRB;
    private void Start()
    {
        selfRB = GetComponent<Rigidbody>();
        if (comOverride != null)
            selfRB.centerOfMass = comOverride.position;
    }
}
