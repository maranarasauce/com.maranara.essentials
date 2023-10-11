using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Milksoup.Meta
{
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
                Destroy(gameObject);
        }
    }
}
