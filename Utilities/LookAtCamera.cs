using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    /// <summary>
    /// Makes the attached Transform look directly at the camera, perfect for billboards
    /// </summary>
    public class LookAtCamera : MonoBehaviour
    {
        Transform camera;
        [Tooltip("If the looking axis is incorrect, you can offset the LookRotation here")] public Vector3 offset = Vector3.zero;

        private void Start()
        {
            camera = Camera.main.transform;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(camera.position - transform.position, Vector3.up) * Quaternion.Euler(offset);
        }
    }

}
