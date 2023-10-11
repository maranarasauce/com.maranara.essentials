using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
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

}
