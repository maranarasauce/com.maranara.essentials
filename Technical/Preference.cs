using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    [CreateAssetMenu(fileName = "Preference.asset", menuName = "ScriptableObjects/Maranara/Preference", order = 0)]
    public class Preference : ScriptableObject
    {
        public string id;
        public DataType type;
        public enum DataType
        {
            Integer,
            Float,
            String
        }
        public int defaultInt;
        public float defaultFloat;
        public string defaultString;

        public string GetString()
        {
            return PlayerPrefs.GetString(id, defaultString);
        }

        public float GetFloat()
        {
            return PlayerPrefs.GetFloat(id, defaultFloat);
        }

        public int GetInt()
        {
            return PlayerPrefs.GetInt(id, defaultInt);
        }
    }

}
