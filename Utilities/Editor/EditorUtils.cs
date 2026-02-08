#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class EditorUtils
{
	public static VisualElement DefaultInspector(SerializedObject serializedObject)
	{
		VisualElement root = new VisualElement();
		var iterator = serializedObject.GetIterator();
		if (iterator.NextVisible(true))
		{
			do
			{
				var propertyField = new PropertyField(iterator.Copy()) { name = iterator.propertyPath };

				if (iterator.propertyPath == "m_Script" && serializedObject.targetObject != null)
					propertyField.SetEnabled(value: false);

				root.Add(propertyField);
			}
			while (iterator.NextVisible(false));
		}
		return root;
	}

	public static Foldout UsageInfo(string text)
	{
		Foldout usage = new Foldout();
		usage.text = "Usage Info";
		usage.value = false;
		Label info = new Label(text);
		info.style.whiteSpace = WhiteSpace.Normal;
		usage.Add(info);
		return usage;
	}
}
#endif