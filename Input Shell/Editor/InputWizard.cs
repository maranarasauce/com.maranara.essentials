using JetBrains.Annotations;
using Maranara.InputShell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

[CustomEditor(typeof(InputSet))]
public class InputSet_Inspector : Editor
{
    const string visualTreeGUID = "b3e2f597980fdc046a1fa6886a852f16";
    const string actionSetGUID = "f290fdfb2fc8186478c13a341f98e027";
    public override VisualElement CreateInspectorGUI()
    {
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(visualTreeGUID));
        // Create a new VisualElement to be the root of our inspector UI
        VisualElement myInspector = new VisualElement();

        // Load from default reference
        visualTree.CloneTree(myInspector);
        root = myInspector;

        root.Q<Button>("save").clicked += InputSet_Inspector_clicked;
        root.Q<Button>("load").clicked += RetrieveScript;

        actionDaddy = root.Q<VisualElement>("actionset");
        actionTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(actionSetGUID));
        InitActionSet();

        Button btn = myInspector.Q<Button>("addaction");
        btn.clicked += AddAction;

        // Return the finished inspector UI
        return myInspector;
    }

    private void InputSet_Inspector_clicked()
    {
        InputSet set = (InputSet)target;
        string path = Directory.GetParent(AssetDatabase.GetAssetPath(set)).FullName;
        GenerateScript(Path.Combine(path, $"{set.SetName}.cs"));
    }

    private VisualElement actionDaddy;
    private VisualElement root;
    private VisualTreeAsset actionTree;
    public int test;
    void AddAction()
    {
        InputSet set = (InputSet)target;
        set.AddAction(set.NewActionName, set.NewActionType, set.NewActionMirrored);
        UpdateActionSet();
    }

    private void InitActionSet()
    {
        InputSet set = (InputSet)target;

        if (set.actions == null)
            set.actions = new List<InputSet.Action>();
        for (int i = 0; i < set.actions.Count; i++)
        {
            InputSet.Action action = set.actions[i];
            ConstructInfoElement(action);
        }
    }

    private void UpdateActionSet()
    {
        if (actionTree == null)
            return;
        InputSet set = (InputSet)target;

        for (int i = 0; i < actionDaddy.childCount; i++)
        {
            VisualElement element = actionDaddy.ElementAt(i);
            bool hasIt = false;
            for (int x = 0; x < set.actions.Count; x++)
            {
                InputSet.Action action = set.actions[x];

                if (action.Name == element.name)
                    hasIt = true;
            }
            if (!hasIt)
                actionDaddy.RemoveAt(i);
        }

        for (int i = 0; i < set.actions.Count; i++)
        {
            InputSet.Action action = set.actions[i];
            
            if (actionDaddy.Q<VisualElement>(action.Name) == null)
            {
                ConstructInfoElement(action);
            }
        }

        actionDaddy.MarkDirtyRepaint();
        EditorUtility.SetDirty(target);
    }

    private void ConstructInfoElement(InputSet.Action action)
    {
        VisualElement info = actionTree.Instantiate();
        info.name = action.Name;
        info.Q<Label>("actionname").text = action.Name;
        info.Q<Button>("actiontype").text = action.Type.ToString();
        info.Q<Button>("removeaction").clicked += () => RemoveAction(action.Name);
        info.Q<Toggle>("actionmirrored").value = action.Mirrored;
        actionDaddy.Add(info);
    }

    void RemoveAction(string name)
    {
        InputSet set = (InputSet)target;
        set.RemoveAction(name);
        UpdateActionSet();
    }

    private void GenerateScript(string path)
    {
        InputSet set = (InputSet)target;
        string text = $"using Maranara.InputShell;\r\n\r\npublic class {set.SetName} : BaseInput\r\n{{\r\n\r\n";

        List<string> mirroredGuys = new List<string>();
        foreach (InputSet.Action action in set.actions)
        {
            var type = NameToEnumHash.FirstOrDefault(x => x.Value == action.Type).Key;

            string actionText = $"public {type} {action.Name};\n";
            if (action.Mirrored)
                mirroredGuys.Add(actionText);
            else text += actionText;
        }

        if (mirroredGuys.Count > 0)
        {
            text += "public class Controller\r\n    {"; ;
            foreach (string mirror in mirroredGuys)
            {
                text += mirror;
            }
            text += "}\n";
            text += "private Controller controllerL;\n";
            text += "private Controller controllerR;\n";
            text += "public Controller GetHand(bool left) { return left ? controllerL : controllerR; }\n";
        }

        text += "}";

        File.WriteAllText(path, text);
        AssetDatabase.Refresh();
    }

    private static Dictionary<string, InputSet.ActionType> NameToEnumHash = new Dictionary<string, InputSet.ActionType>()
    {
        {"InputDigital", InputSet.ActionType.Digital },
        {"OutputHaptic", InputSet.ActionType.Haptic},
        { "InputJoystick", InputSet.ActionType.Joystick},
        { "InputAxis",InputSet.ActionType.Axis} ,
        { "InputPose", InputSet.ActionType.Pose}
    };

    private void AddFieldInfo(MemberInfo info, ref List<InputSet.Action> actions, bool mira)
    {
        if (info.MemberType != MemberTypes.Field)
            return;
        FieldInfo fieldInfo = (FieldInfo)info;

        if (NameToEnumHash.TryGetValue(fieldInfo.FieldType.Name, out InputSet.ActionType value))
        {
            InputSet.Action action = new InputSet.Action()
            {
                Name = info.Name,
                Type = value,
                Mirrored = mira
            };
            actions.Add(action);
        }
    }

    private void RetrieveScript()
    {
        InputSet set = (InputSet)target;

        Type type = set.generatedSet.GetClass();
        set.SetName = type.Name;

        List<InputSet.Action> actions = new List<InputSet.Action>();
        MemberInfo[] infos = type.GetMembers();
        foreach (MemberInfo info in infos)
        {
            if (info.MemberType == MemberTypes.NestedType)
            {
                TypeInfo tInfo = (TypeInfo)info;
                if (tInfo.Name == "Controller")
                {
                    foreach (MemberInfo subInfo in tInfo.GetMembers())
                    {
                        AddFieldInfo(subInfo, ref actions, true);
                    }
                }
            }

            AddFieldInfo(info, ref actions, false);
        }

        set.actions = actions;
        UpdateActionSet();
    }
}
