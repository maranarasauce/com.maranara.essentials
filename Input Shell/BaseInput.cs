using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.InputShell
{
    /*
     * CONTROLLER LAYOUT - : https://i.imgur.com/zNBGIUB.png : -
     */
    public delegate void OnBoolUpdate(bool pressed);
    public delegate void OnFloatUpdate(float value);
    public delegate void OnFloat2Update(Vector2 value);
    public delegate void OnFloatArrayUpdate(float[] value);
    public delegate void OnInputUpdate();
    public enum Controller
    {
        LeftHand,
        RightHand,
        Chest,
        Head
    }

    public class BaseInput : MonoBehaviour
    {

    }
}
