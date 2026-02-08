#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(Relay))]
[CanEditMultipleObjects]
public class RelayEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement element = EditorUtils.DefaultInspector(serializedObject);
        var childs = element.Children();
        VisualElement disableEvent = null;
		PropertyField activationType = null;
        foreach (var child in childs)
        {
            PropertyField property = child as PropertyField;
            switch (property.name)
            {
                case "DisableEvent":
                    disableEvent = child;
                    break;
                case "activationType":
                    activationType = property;
                    break;
            }
        }

		activationType.RegisterValueChangeCallback((evt) => ActivationTypeChanged(evt, disableEvent));
        UpdateDisableEvent(disableEvent);

		return element;
    }

    private void UpdateDisableEvent(VisualElement disableEvent)
    {
		Relay.ActivationTrigger enumFlag = (Relay.ActivationTrigger)serializedObject.FindProperty("activationType").enumValueFlag;
        if ((enumFlag & Relay.ActivationTrigger.Disabled) == Relay.ActivationTrigger.None)
        {
            disableEvent.style.display = DisplayStyle.None;
        }
        else disableEvent.style.display = DisplayStyle.Flex;
	}

    private void ActivationTypeChanged(SerializedPropertyChangeEvent evt, VisualElement disableEvent)
    {
        UpdateDisableEvent(disableEvent);
    }
}
#endif