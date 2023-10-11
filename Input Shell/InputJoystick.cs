using UnityEngine;

namespace Maranara.InputShell
{
    public class InputJoystick
    {
        public string name { get; private set; }
        public InputAxis xAxis { get; private set; }
        public InputAxis yAxis { get; private set; }
        public event OnFloat2Update axisChanged;
        public Vector2 value { get; private set; }
        public InputJoystick(string name)
        {
            this.name = name;
            xAxis = new InputAxis(name + "X");
            yAxis = new InputAxis(name + "Y");
        }

        public void Update(Vector2 newValue)
        {
            if (value != newValue)
            {
                value = newValue;
                axisChanged?.Invoke(value);

                float valueX = newValue.x;
                float valueY = newValue.y;

                xAxis?.Update(valueX);
                yAxis?.Update(valueY);
            }
        }

        public void Update(float xValue, float yValue)
        {
            Vector2 newVec = value;
            newVec.x = xValue;
            newVec.y = yValue;

            if (value != newVec)
            {
                value = newVec;
                axisChanged?.Invoke(value);

                xAxis?.Update(xValue);
                yAxis?.Update(yValue);
            }
        }
    }
}