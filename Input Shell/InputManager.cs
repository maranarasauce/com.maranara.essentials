using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maranara.InputShell
{
    public class InputManager : MonoBehaviour
    {
        public BaseInput InputSet;
        public static InputManager instance;
        [Serializable]
        public struct PluginPair
        {
            public RuntimePlatform platform;
            public InputPlugin plugin;
        }
        [SerializeField] PluginPair[] Plugins;
        private InputPlugin activePlugin;

        public void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            activePlugin = Plugins.FirstOrDefault(x => Application.platform == x.platform).plugin;
            activePlugin.ActiveInput = InputSet;
            foreach (PluginPair plugin in Plugins)
            {
                if (plugin.plugin != activePlugin)
                {
                    Destroy(plugin.plugin);
                }
            }
        }

        public T GetInput<T>() where T : BaseInput
        {
            return (T)InputSet;
        }

        private void OnEnable()
        {
            activePlugin.InputEnabled();
        }

        private void OnDisable()
        {
            activePlugin.InputDisabled();
        }

        private void Update()
        {
            activePlugin.InputUpdate();
        }
    }

}
