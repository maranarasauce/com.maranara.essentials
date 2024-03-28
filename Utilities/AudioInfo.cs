using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Maranara.Utility
{
    /// <summary>
    /// Stores audio randomness and configuration, making script's audio customization much less boilerplate centric
    /// </summary>
    [Serializable]
    public struct AudioInfo
    {

        public AudioClip[] clips;
        [Range(0f, 1f)]
        public float Volume;
        [Range(0f, 1f)]
        public float Pitch;
        [Range(0f, 0.5f)]
        public float VolumeVariance;
        [Range(0f, 0.5f)]
        public float PitchVariance;
        public bool Spatial;
        [Range(0f, 360f)]
        public float Spread;
        [Range(0f, 1f)]
        public float Doppler;
        public float MinDistance;
        public float MaxDistance;
        [HideInInspector]
        public bool Initialized;

        public AudioClip GetRandomClip()
        {
            if (clips.Length == 1)
                return GetFirstClip();
            return clips[Random.Range(0, clips.Length - 1)];
        }

        public AudioClip GetFirstClip()
        {
            return clips[0];
        }
        public AudioInfo(AudioClip[] clips, float volume, float pitch)
        {
            this.clips = clips;
            this.Volume = volume;
            this.Pitch = pitch;
            this.VolumeVariance = 0f;
            this.PitchVariance = 0f;
            this.Spatial = true;
            this.Spread = 0f;
            this.Doppler = 1f;
            this.MinDistance = 1f;
            this.MaxDistance = 500f;
            this.Initialized = true;
        }
    }
}
