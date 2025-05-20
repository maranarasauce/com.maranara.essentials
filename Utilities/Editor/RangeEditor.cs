using Maranara.Utility;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Range))]
public class RangeEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var minLabelRect = new Rect(position.x, position.y, 25, position.height);
        var minRect = new Rect(position.x + 30, position.y, 50, position.height);
        var maxLabelRect = new Rect(position.x + 100, position.y, 25, position.height);
        var maxRect = new Rect(position.x + 130, position.y, 50, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.LabelField(minLabelRect, "Min");
        EditorGUI.LabelField(maxLabelRect, "Max");
        EditorGUI.PropertyField(minRect, property.FindPropertyRelative("Minimum"), GUIContent.none);
        EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("Maximum"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
