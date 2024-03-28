using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    /// <summary>
    /// Modulates the attached AudioSource based on the described transform's movement
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class PrismaticSFX : MonoBehaviour
    {
        [Tooltip("Transform that the script samples velocity from.")][SerializeField] Transform sampleTransform;
        [Tooltip("Maximum volume allowed by the script. Scales the output volume accordingly.")] [SerializeField] float maxVolume = 1f;
        [Tooltip("Typical velocity of the sample Transform.")] [SerializeField] float averageVelocity = 20f;
        [Tooltip("Makes the output pitch higher or lower.")] [SerializeField] float pitchScalar = 1.5f;
        [Tooltip("The lowest the output pitch allows.")] [SerializeField] float minPitch = 0.8f;
        [Tooltip("The highest the output pitch allows.")][SerializeField] float maxPitch = 1.5f;
        [Tooltip("Smooths out the sampled velocity.")] [SerializeField] float smoothness = 3f;
        private AudioSource _src;
        private void Awake()
        {
            _src = GetComponent<AudioSource>();
            _src.loop = true;
            _src.Play();
            _src.volume = 0f;
        }

        Vector3 lastPos;
        float lastCalc;

        private void OnDisable()
        {
            _src.volume = 0f;
        }

        private void Update()
        {
            Vector3 pos = sampleTransform.position;
            float time = Time.deltaTime;
            if (time == 0)
                time = 1f;
            Vector3 vel = (pos - lastPos) / time;
            float dist = vel.magnitude;

            float calc = Mathf.Clamp(dist / averageVelocity, 0f, 1f);
            calc = Mathf.Lerp(lastCalc, calc, Time.deltaTime * smoothness);

            _src.volume = calc * maxVolume;
            _src.pitch = Mathf.Clamp(calc * pitchScalar, minPitch, maxPitch);

            lastPos = pos;
            lastCalc = calc;
        }
    }

}
