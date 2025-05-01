using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Maranara.Utility
{
    [RequireComponent(typeof(Slider))]
    public class SliderLink : PrefsLink
    {
        private Slider slider;
        [SerializeField] TextMeshProUGUI valueDisplay;
        [SerializeField] int displayPrecision = 1;
        [NonSerialized] public UnityEvent OnPointerDown = new UnityEvent();
        [NonSerialized] public UnityEvent OnPointerUp = new UnityEvent();

        public void PointerDown()
        {
            OnPointerDown?.Invoke();
        }

        public void PointerUp()
        {
            OnPointerUp?.Invoke();
        }

        private void Awake()
        {
            OnInit();
        }

        public override void OnInit()
        {
            base.OnInit();
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(OnValueChanged);
            GetPref();
            OnPrefApplied();
        }

        private void OnValueChanged(float value)
        {
            SetPref(value);
            valueDisplay.text = GetTruncatedString(value);
        }

        public override void OnPrefApplied()
        {
            base.OnPrefApplied();
            if (_savedPref == null)
                return;

            float value = (float)_savedPref;
            slider.value = value;
            valueDisplay.text = GetTruncatedString(value);
        }

        private string GetTruncatedString(float value)
        {
            string formatString = "F";
            formatString += displayPrecision;
            return value.ToString(formatString);
        }
    }
}