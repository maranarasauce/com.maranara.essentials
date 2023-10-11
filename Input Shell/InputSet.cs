using Maranara.InputShell;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Input Set", menuName = "ScriptableObjects/Maranara/Input Set", order = 1)]
public class InputSet : ScriptableObject
{
    [Serializable]
    public struct Action
    {
        public ActionType Type;
        public string Name;
        public bool Mirrored;
    }

    public enum ActionType
    {
        Haptic,
        Digital,
        Joystick,
        Axis,
        Pose
    }

    [SerializeField] public MonoScript generatedSet;
    [SerializeField] public List<Action> actions;
    
    public string NewActionName;
    public InputSet.ActionType NewActionType;
    public bool NewActionMirrored;
    public string SetName;
    public void RemoveAction(string name)
    {
        //actions.RemoveAt(offset);
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].Name == name)
            {
                actions.RemoveAt(i);
                return;
            }
                
        }
    }

    public void AddAction(string name, ActionType type, bool mirror)
    {
        foreach (Action test in actions)
        {
            if (test.Name == name)
            {
                Debug.Log("Rejected due to having the same name");
                return;
            }
        }

        Action action = new Action()
        {
            Name = name,
            Type = type,
            Mirrored = mirror
        };

        actions.Add(action);
    }
}
