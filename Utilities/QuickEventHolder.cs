using Maranara.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickEventHolder : MonoBehaviour
{
    [SerializeField] QuickEvent Event;
    [SerializeField] bool onEnable = true;

    private void OnEnable()
    {
        if (onEnable) Invoke();
    }

    public void Invoke()
    {
        Event.Invoke(this);
    }
}
