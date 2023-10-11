using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Maranara.General;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

namespace Screamies.Utility
{
    public enum FullPoolHandleType
    {
        Repool,
        ExpandPool,
        Instantiate,
        DoNothing
    }

    public class PoolManager : MonoBehaviour
    {
        #region Singleton
        public static PoolManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void OnLevelWasLoaded(int level)
        {
            Instance = this;
        }
        #endregion

        public Dictionary<string, Queue<GameObject>> poolDictionary;
        public static void SpawnObject(string key, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Instance.SpawnProcess(key, position, rotation, scale);
        }

        private async UniTask<GameObject> SpawnProcess(string key, Vector3 pos, Quaternion rot, Vector3 big)
        {
            AsyncOperationHandle<GameObject> result = Addressables.LoadAssetAsync<GameObject>(key);
            await result;
            GameObject inst = GameObject.Instantiate(result.Result, pos, rot);
            inst.transform.localScale = big;
            return inst;
        }

        public static void SpawnObject(string key, Vector3 position)
        {
            SpawnObject(key, position, Quaternion.identity, Vector3.one);
        }

        private const string AUDIO_KEY = "Interaction.Audio";
        public static void PlayAudio(AudioInfo info, Vector3 position)
        {
            Instance.AudioProcess(info, position);
        }

        public async UniTask AudioProcess(AudioInfo info, Vector3 pos)
        {
            GameObject inst = await SpawnProcess(AUDIO_KEY, pos, Quaternion.identity, Vector3.zero);
            DestroyAfterTime time = inst.GetComponent<DestroyAfterTime>();
            AudioSource src = inst.GetComponent<AudioSource>();
            time.Init(src);
            ApplyAudioSource(src, info);
        }

        public void ApplyAudioSource(AudioSource src, AudioInfo info)
        {
            src.clip = info.clips.Random();
            src.volume = info.Volume + Random.Range(-info.VolumeVariance, info.VolumeVariance);
            src.pitch = info.Pitch + Random.Range(-info.PitchVariance, info.PitchVariance);
            src.spatialBlend = info.Spatial ? 1f : 0f;
            src.spread = info.Spread;
            src.dopplerLevel = info.Doppler;
            src.minDistance = info.MinDistance;
            src.maxDistance = info.MaxDistance;
        }
    }
}
