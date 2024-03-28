using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    /// <summary>
    /// Good class for changing Rigidbody properties.
    /// </summary>
    public class RigidbodySettings : MonoBehaviour
    {
        [Tooltip("Overrides the Rigidbody's Center of Mass.")]  public Transform comOverride;
        Rigidbody selfRB;
        private void Start()
        {
            selfRB = GetComponent<Rigidbody>();
            if (comOverride != null)
                selfRB.centerOfMass = comOverride.position;
        }
    }

}
