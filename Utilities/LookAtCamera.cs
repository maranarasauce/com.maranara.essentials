using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    public class LookAtCamera : MonoBehaviour
    {
        Transform camera;
        public Quaternion offset = Quaternion.identity;

        private void Start()
        {
            camera = Camera.main.transform;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(camera.position - transform.position, Vector3.up) * offset;
        }
    }

}
