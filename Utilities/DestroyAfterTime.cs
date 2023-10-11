using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private bool disableInstead;
        [SerializeField] private float delay;
        [SerializeField] private AudioSource referenceLength;
        private float time;

        public void Init(AudioSource src)
        {
            referenceLength = src;
            delay = referenceLength.clip.length;

        }

        private void Start()
        {
            if (referenceLength != null)
                Init(referenceLength);
            time = delay;
        }

        private void Update()
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                End();
            }
        }

        private void End()
        {
            if (disableInstead)
                gameObject.SetActive(false);
            else Destroy(gameObject);
        }
    }
}   