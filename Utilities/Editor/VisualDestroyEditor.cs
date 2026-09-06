#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UIElements;

[CanEditMultipleObjects]
[CustomEditor(typeof(VisualDestroy))]
public class VisualDestroyEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
		VisualDestroy visDes = (VisualDestroy)target;

        VisualElement ins = new VisualElement();
        if (visDes.fx == null)
        {
            visDes.fx = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath("9eda0e1ddd155cd498df419eda98214b"));
            UnityEditor.EditorUtility.SetDirty(visDes);
        }

		InspectorElement.FillDefaultInspector(ins, serializedObject, this);
		return ins;
    }
}
#endif