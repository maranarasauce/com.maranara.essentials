using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maranara.InputShell
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;
        [Serializable]
        public struct PluginPair
        {
            public RuntimePlatform platform;
            public InputPlugin plugin;
        }
        [SerializeField] PluginPair[] plugins;
        private InputPlugin activePlugin;

        public void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            activePlugin = plugins.FirstOrDefault(x => Application.platform == x.platform).plugin;
            foreach (PluginPair plugin in plugins)
            {
                if (plugin.plugin != activePlugin)
                {
                    Destroy(plugin.plugin);
                }
            }
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
