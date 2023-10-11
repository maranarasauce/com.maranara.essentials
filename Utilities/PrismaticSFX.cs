using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    [RequireComponent(typeof(AudioSource))]
    public class PrismaticSFX : MonoBehaviour
    {
        [SerializeField] float volumeCoeff = 1f;
        [SerializeField] Transform tsf;
        [SerializeField] float velDiv = 20f;
        [SerializeField] float pitchCoeff = 1.5f;
        [SerializeField] float minPitch = 0.8f;
        [SerializeField] float maxPitch = 1.5f;
        [SerializeField] float dampTime = 3f;
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
            Vector3 pos = tsf.position;
            float time = Time.deltaTime;
            if (time == 0)
                time = 1f;
            Vector3 vel = (pos - lastPos) / time;
            float dist = vel.magnitude;

            float calc = Mathf.Clamp(dist / velDiv, 0f, 1f);
            calc = Mathf.Lerp(lastCalc, calc, Time.deltaTime * dampTime);

            _src.volume = calc * volumeCoeff;
            _src.pitch = Mathf.Clamp(calc * pitchCoeff, minPitch, maxPitch);

            lastPos = pos;
            lastCalc = calc;
        }
    }

}
