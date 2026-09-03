using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Maranara.Utility
{
    [Serializable]
    public struct QuickEvent
    {
        [SerializeField] UnityEvent Event;
        [SerializeField] GameObject[] EnableObjects;
        [SerializeField] GameObject[] DisableObjects;
        [SerializeField] float delay;
        /// <param name="behaviour"> Required so delay is possible
        public void Invoke(MonoBehaviour behaviour)
        {
            behaviour.StartCoroutine(Do());
        }

        private IEnumerator Do()
        {
            yield return new WaitForSeconds(delay);
            Event?.Invoke();
            foreach (var obj in EnableObjects) obj.SetActive(true);
            foreach (var obj in DisableObjects) obj.SetActive(false);
        }
    }

}
