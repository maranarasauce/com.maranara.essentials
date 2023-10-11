using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    public class TransformLineRenderer : MonoBehaviour
    {
        public Transform first;
        public Transform last;
        public LineRenderer renderer;

        private void Update()
        {
            renderer.SetPosition(0, renderer.transform.InverseTransformPoint(first.position));
            renderer.SetPosition(1, renderer.transform.InverseTransformPoint(last.position));
        }
    }
}