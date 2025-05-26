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
        private BoolTimer safeBuffer;
        private Renderer rend;
        private void Start()
        {
            rend = gameObject.GetComponent<Renderer>();
            
        }

        private void OnEnable()
        {
            safeBuffer.Set(0.1f);
        }

        private void Update()
        {
            if (!rend.isVisible && !safeBuffer)
                gameObject.SetActive(false);
        }
    }
}
