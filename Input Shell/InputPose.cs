namespace Maranara.InputShell
{
    public class InputPose
    {
        public string name { get; private set; }
        public event OnFloatArrayUpdate axisChanged;
        public float[] value { get; private set; }

        public InputPose(string name)
        {
            this.name = name;
        }

        public void Update(float[] newValue)
        {
            if (value != newValue)
            {
                value = newValue;
                axisChanged?.Invoke(value);
            }
        }
    }
}