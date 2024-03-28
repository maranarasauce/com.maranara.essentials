using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    /// <summary>
    /// Disables the attached GameObject after a defined amount of time
    /// </summary>
    public class DestroyAfterTime : MonoBehaviour
    {
        [Tooltip("Disable the GameObject instead of destroying")] [SerializeField] private bool disableInstead;
        [Tooltip("The delay before destruction in seconds")] [SerializeField] private float delay;
        [Tooltip("If filled, the GameObject destroys after AudioSource clip's length")] [SerializeField] private AudioSource referenceLength;
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