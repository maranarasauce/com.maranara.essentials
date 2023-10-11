using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    public class ConstantMotion : MonoBehaviour
    {
        [SerializeField] private Vector3 positional;
        [SerializeField] private Vector3 rotational;
        public bool scaleTime;

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