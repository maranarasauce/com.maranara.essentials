using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    public class TransformLineRenderer : MonoBehaviour
    {
        public Transform first;
        public Transform last;
        public LineRenderer lr;

        private void Update()
        {
            lr.SetPosition(0, lr.transform.InverseTransformPoint(first.position));
            lr.SetPosition(1, lr.transform.InverseTransformPoint(last.position));
        }
    }
}