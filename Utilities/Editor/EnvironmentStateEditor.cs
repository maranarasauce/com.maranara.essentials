#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnvironmentState))]
public class EnvironmentChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnvironmentState h = target as EnvironmentState;
        base.OnInspectorGUI();
        if (GUILayout.Button("Save Current Environment"))
		{
			Undo.RecordObject(h, "Environment Change");
			h.GetSettings();
            EditorUtility.SetDirty(h);
        }
        if (GUILayout.Button("Apply Environment"))
        {
            h.SetSettings();

        }
    }
}
#endif