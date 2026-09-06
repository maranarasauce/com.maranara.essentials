using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Nodder : MonoBehaviour
{
    public float speed = 1f;
    public float strength = 1f;
    public Vector3 position;
    public Vector3 rotation;
    public bool absolute;
    private Vector3 oP;
    private Vector3 oR;

    private void Awake()
    {
        SetOriginal();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetOriginal()
    {
        oP = transform.localPosition;
        oR = transform.localEulerAngles;
    }

    float t = 0f;
    private void Update()
    {
        t += Time.deltaTime;
        float x = Mathf.Sin(t * speed);
        if (absolute)
            x = Mathf.Abs(x);
        transform.localPosition = oP + (x * position * strength);
        transform.localRotation = Quaternion.Euler(oR + (x * rotation * strength));
    }

#if UNITY_EDITOR
    float timeStart = -1;
    float thickness = 6f;
    private void OnDrawGizmosSelected()
    {
        if (timeStart < 0f)
            timeStart = (float)EditorApplication.timeSinceStartup;

        float dT = (float)EditorApplication.timeSinceStartup - timeStart;

        float x = Mathf.Sin(dT * speed);
        if (absolute)
            x = Mathf.Abs(x);
        Vector3 pos = transform.localPosition + (x * position * strength);
        Quaternion lRot = Quaternion.Euler(transform.localEulerAngles + (x * rotation * strength));

        if (transform.parent != null)
        {
            pos = transform.parent.localToWorldMatrix.MultiplyPoint(pos);
            lRot = lRot * transform.parent.localToWorldMatrix.rotation;
        }

        thickness = 15f;
        Vector3 f = pos + (lRot * Vector3.forward * 0.5f);
        Handles.DrawBezier(pos, f, pos, f, Color.cyan, null, thickness);
        Vector3 u = pos + (lRot * Vector3.up * 0.5f);
        Handles.DrawBezier(pos, u, pos, u, Color.yellow, null, thickness);
        Vector3 r = pos + (lRot * Vector3.right * 0.5f);
        Handles.DrawBezier(pos, r, pos, r, Color.magenta, null, thickness);
    }
#endif
}
