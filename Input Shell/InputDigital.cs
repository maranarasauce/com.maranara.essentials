namespace Maranara.InputShell
{
    public class InputDigital
    {
        public string name { get; private set; }
        public event OnBoolUpdate stateChanged;
        public event OnInputUpdate onStateDown;
        public event OnInputUpdate onStateUp;
        public bool value { get; private set; }

        public void Update(bool newValue)
        {
            if (value != newValue)
            {
                value = newValue;

                stateChanged?.Invoke(value);
                if (value)
                    onStateDown?.Invoke();
                else onStateUp?.Invoke();

            }
        }

        public InputDigital(string name)
        {
            this.name = name;
        }
    }
}