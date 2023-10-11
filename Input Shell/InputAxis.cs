namespace Maranara.InputShell
{
    public class InputAxis
    {
        public string name { get; private set; }
        public event OnFloatUpdate axisChanged;
        public float value { get; private set; }

        public InputAxis(string name)
        {
            this.name = name;
        }

        public void Update(float newValue)
        {
            if (value != newValue)
            {
                value = newValue;
                axisChanged?.Invoke(value);
            }
        }
    }
}