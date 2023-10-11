using System;
using UnityEngine;

namespace Maranara.InputShell
{
    public class OutputHaptic
    {
        public string name { get; private set; }
        public Controller controller;

        public OutputHaptic(string name, Controller controllerNumber)
        {
            this.name = name;
            this.controller = controllerNumber;
        }

        public Action<Vector2, Controller> vibrate;
        public void Vibrate(float intensity, float duration)
        {
            Vector2 value = new Vector2(intensity, duration);
            vibrate?.Invoke(value, controller);
        }
    }
}