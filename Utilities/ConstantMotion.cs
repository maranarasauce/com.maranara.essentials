using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    /// <summary>
    /// Move the attached object constantly, perfect for environmental art
    /// </summary>
    public class ConstantMotion : MonoBehaviour
    {
        [Tooltip("How often the object moves per second")] [SerializeField] private Vector3 positional;
        [Tooltip("How often the object rotates per second")][SerializeField] private Vector3 rotational;
        
        [Tooltip("If false, script will move per frame instead of per second")] public bool scaleTime = true;

        private void Update()
        {
            Vector3 a = positional;
            Vector3 b = rotational;
            if (scaleTime)
            {
                a *= Time.deltaTime;
                b *= Time.deltaTime;
            }

            transform.position += a;
            transform.rotation *= Quaternion.Euler(b);
        }
    }

}