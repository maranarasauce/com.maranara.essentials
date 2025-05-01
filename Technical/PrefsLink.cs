using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Maranara.Utility
{
    public class PrefsLink : MonoBehaviour
    {
        public Preference preference;
        [NonSerialized] public object _currentPref;
        [NonSerialized] public object _savedPref;

        public static UnityEvent OnPrefsSaved = new UnityEvent();
        public static UnityEvent OnPrefsReverted = new UnityEvent();

        public static UnityEvent OnPrefsChanged = new UnityEvent();
        public static UnityEvent OnPrefsApplied = new UnityEvent();
        public static void Save()
        {
            PlayerPrefs.Save();
            OnPrefsSaved?.Invoke();
        }

        public static void Clear()
        {
            PlayerPrefs.DeleteAll();
            /*Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif*/
        }

        public static void Revert()
        {
            OnPrefsReverted?.Invoke();
        }

        private void Start()
        {
            OnPrefsSaved.AddListener(ApplyPref);
            OnPrefsReverted.AddListener(RevertPref);
            OnInit();
        }

        public virtual void OnInit()
        {

        }

        public void GetPref()
        {
            if (preference.type == Preference.DataType.Float)
                _savedPref = preference.GetFloat();
            else if (preference.type == Preference.DataType.Integer)
                _savedPref = preference.GetInt();
            else if (preference.type == Preference.DataType.String)
                _savedPref = preference.GetString();

            _currentPref = _savedPref;
        }

        public void SetPref(object newValue)
        {

            if (preference.type == Preference.DataType.Float)
                PlayerPrefs.SetFloat(preference.id, (float)newValue);
            else if (preference.type == Preference.DataType.Integer)
                PlayerPrefs.SetInt(preference.id, (int)newValue);
            else if (preference.type == Preference.DataType.String)
                PlayerPrefs.SetString(preference.id, (string)newValue);

            _currentPref = newValue;

            OnPrefsChanged?.Invoke();
        }

        public void ApplyPref()
        {
            _savedPref = _currentPref;
            SetPref(_savedPref);
            OnPrefApplied();
        }

        public void RevertPref()
        {
            _currentPref = _savedPref;
            OnPrefApplied();
        }

        public virtual void OnPrefApplied()
        {
            OnPrefsApplied?.Invoke();
        }
    }

}
