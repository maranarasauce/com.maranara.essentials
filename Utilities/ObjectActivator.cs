using Cysharp.Threading.Tasks;
using UltEvents;
using UnityEngine;
using UnityEngine.Serialization;

namespace Maranara.Utility
{
    /// <summary>
    /// A wonderful general class useful for designing intermediate-complexity interactions without resorting to code
    /// Activates OnTriggerEnter
    /// </summary>
    public class ObjectActivator : MonoBehaviour
    {
        [Tooltip("True: Only activates once and only disactivates once.")]
        public bool oneTime = true;
        [Tooltip("True: Whenever this script is enabled, it will Activate.")]
        public bool activateOnEnable = true;
        [Tooltip("True: Whenever this script is disabled, it will Disactivate.")]
        public bool disactivateOnDisable = true;
        [Tooltip("Start the activator locked. Upon being locked, it will not activate or disactivate until Unlock is called.")]
        public bool startLocked = false;
        [Tooltip("Any activation/disactivation will be delayed by X seconds.")]
        public float delay = 0f;
        [Tooltip("If true, then toDisable GameObjects will disable upon Activation and toEnable GameObjects will enable upon Disactivation.")]
        [FormerlySerializedAs("toggleTos")] public bool gameObjectsToggle = false;

        [Tooltip("On Activation, these GameObjects will be enabled.")]
        public GameObject[] toEnable;
        [Tooltip("On Disactivation, these GameObjects will be enabled.")]
        public GameObject[] toDisable;
        [Tooltip("On Activation, this event will be invoked.")]
        public UltEvent OnActivate;
        [Tooltip("On Disactivation, this event will be invoked.")]
        public UltEvent OnDisactivate;

        [Tooltip("The LayerMask for objects that enter an ObjectActivator trigger")]
        public LayerMask activationLayer;
        private bool locked;

        private void Awake()
        {
            locked = startLocked;
        }

        public void Unlock()
        {
            locked = false;
        }

        public void Lock()
        {
            locked = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (activationLayer.HasLayer(other.gameObject.layer))
            {
                Activate();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (activationLayer.HasLayer(other.gameObject.layer))
            {
                Disactivate();
            }
        }

        private void OnEnable()
        {
            if (activateOnEnable)
                Activate();
        }

        private void OnDisable()
        {
            if (disactivateOnDisable)
                Disactivate();
        }

        private bool activated;
        private bool disactivated;
        public void Activate()
        {
            if (locked || oneTime && activated)
                return;

            CoActivate();
        }

        private async UniTask CoActivate()
        {
            activated = true;

            if (delay > 0f)
            {
                await UniTask.Delay((int)(delay * 1000));
            }

            foreach (GameObject go in toEnable)
            {
                go.SetActive(true);
            }

            if (gameObjectsToggle)
            {
                foreach (GameObject go in toDisable)
                {
                    go.SetActive(false);
                }
            }

            OnActivate?.Invoke();
        }

        public void Disactivate()
        {
            if (locked || oneTime && disactivated)
                return;
            CoDisactivate();
        }

        private async UniTask CoDisactivate()
        {
            disactivated = true;

            if (delay > 0f)
            {
                await UniTask.Delay((int)(delay * 1000));
            }
            foreach (GameObject go in toDisable)
            {
                go.SetActive(true);
            }
            if (gameObjectsToggle)
            {
                foreach (GameObject go in toEnable)
                {
                    go.SetActive(true);
                }
            }

            OnDisactivate?.Invoke();
        }
    }
}