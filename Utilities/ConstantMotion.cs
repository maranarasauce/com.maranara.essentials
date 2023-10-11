using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.General
{
    public class ConstantMotion : MonoBehaviour
    {
        [SerializeField] private Vector3 positional;
        public bool rotation;
        [SerializeField] private Quaternion rotational;

        private void Update()
        {
            transform.position += positional * Time.deltaTime;
            if (rotation)
                transform.rotation *= rotational;
        }
    }

}