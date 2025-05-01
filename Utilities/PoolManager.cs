using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

namespace Maranara.Utility
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
        #endregion

        public Dictionary<string, Queue<GameObject>> poolDictionary;
        public static void SpawnObject(string key, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Instance.StartCoroutine(Instance.SpawnProcess(key, position, rotation, scale));
        }

        WaitForEndOfFrame frame;
        private IEnumerator<GameObject> SpawnProcess(string key, Vector3 pos, Quaternion rot, Vector3 big)
        {
            AsyncOperationHandle<GameObject> result = Addressables.LoadAssetAsync<GameObject>(key);
            while (!result.IsDone)
                yield return null;
            GameObject inst = GameObject.Instantiate(result.Result, pos, rot);
            inst.transform.localScale = big;
            yield return inst;
        }

        public static void SpawnObject(string key, Vector3 position)
        {
            SpawnObject(key, position, Quaternion.identity, Vector3.one);
        }

        private const string AUDIO_KEY = "Interaction.Audio";
        public static void PlayAudio(AudioInfo info, Vector3 position)
        {
            Instance.StartCoroutine(Instance.AudioProcess(info, position));
        }

        public IEnumerator AudioProcess(AudioInfo info, Vector3 pos)
        {
            IEnumerator<GameObject> ienum = SpawnProcess(AUDIO_KEY, pos, Quaternion.identity, Vector3.zero);
            while (ienum.MoveNext())
                yield return null;
            GameObject inst = ienum.Current;
            AudioSource src = inst.GetComponent<AudioSource>();
            info.ApplyToAudioSource(src);
            DestroyAfterTime time = inst.AddComponent<DestroyAfterTime>();
            time.Init(src);
            src.Play();
        }
    }
}
