using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    /// <summary>
    /// Disables the GameObject when the attached Renderer is not visible to any Camera
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class DisableWhenInvisible : MonoBehaviour
    {

        private Renderer rend;
        private void Start()
        {
            rend = gameObject.GetComponent<Renderer>();
        }

        private void Update()
        {
            if (!rend.isVisible)
                gameObject.SetActive(false);
        }
    }
}
