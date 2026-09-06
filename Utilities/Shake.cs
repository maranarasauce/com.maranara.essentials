using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UIElements;

public class Shake : MonoBehaviour
{
    Vector3 basePosition;
    Quaternion baseRotation;
    public float frequency = 15f;
    public float intensity = 0.3f;
    public float rotationIntensity = 0.3f;
    public float duration = -1f;
    float time;

    private void OnEnable()
    {
        basePosition = transform.localPosition;
        baseRotation = transform.localRotation;
        if (duration > 0f)
            time = duration;
    }

    private void LateUpdate()
    {
        float tf = Time.time * frequency;
        Vector3 perlin = new Vector3(Mathf.PerlinNoise(tf, 0f), Mathf.PerlinNoise(0f, tf), Mathf.PerlinNoise(tf, tf));
        perlin -= Vector3.one * 0.5f;
        perlin = Vector3.Scale(perlin, Vector3.one * 2f);

        transform.localPosition = basePosition + (perlin * intensity);
        transform.localRotation = baseRotation * Quaternion.Euler(perlin * rotationIntensity);
        time -= Time.deltaTime;
        if (duration > 0f && time < 0f)
            enabled = false;
    }

    private void OnDisable()
    {
        transform.localPosition = basePosition;
        transform.localRotation = baseRotation;
    }

#if UNITY_EDITOR
    float timeStart = -1;
    float thickness = 6f;
    private void OnDrawGizmosSelected()
    {
        if (Selection.activeGameObject != gameObject)
            return;

        if (timeStart < 0f)
            timeStart = (float)EditorApplication.timeSinceStartup;

        float tf = ((float)EditorApplication.timeSinceStartup - timeStart) * frequency;

        Vector3 perlin = new Vector3(Mathf.PerlinNoise(tf, 0f), Mathf.PerlinNoise(0f, tf), Mathf.PerlinNoise(tf, tf));
        perlin -= Vector3.one * 0.5f;
        perlin = Vector3.Scale(perlin, Vector3.one * 2f);

        Vector3 pos = transform.localPosition + (perlin * intensity);
        Quaternion lRot = transform.localRotation * Quaternion.Euler(perlin * rotationIntensity);

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
