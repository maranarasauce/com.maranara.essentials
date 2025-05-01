using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct AudioInfo
{
    public AudioClip[] clips;
    [Range(0f, 1f)]
    public float Volume;
    [Range(0f, 1f)]
    public float Pitch;
    public AudioPreset Preset;
    [HideInInspector]
    public bool Initialized;

    public void ApplyToAudioSource(AudioSource src)
    {
        src.clip = GetRandomClip();
        if (src.clip == null)
            return;

        src.volume = Volume + Random.Range(-Preset.VolumeVariance, Preset.VolumeVariance);
        src.pitch = Pitch + Random.Range(-Preset.PitchVariance, Preset.PitchVariance);
        src.spatialBlend = Preset.Spatial ? 1f : 0f;
        src.minDistance = Preset.MinDistance;
        src.maxDistance = Preset.MaxDistance;
        src.spread = Preset.Spread;
        src.dopplerLevel = Preset.Doppler;
        
    }

    public AudioClip GetRandomClip()
    {
        if (clips.Length == 0)
            return null;

        if (clips.Length == 1)
            return GetFirstClip();
        return clips[Random.Range(0, clips.Length)];
    }

    public AudioClip GetFirstClip()
    {
        return clips[0];
    }
    public AudioInfo(AudioClip[] clips, float volume, float pitch)
    {
        this.clips = clips;
        this.Volume = volume;
        this.Pitch = pitch;
        this.Preset = null;
        this.Initialized = true;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(AudioInfo))]
public class AudioInfoDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);

        SerializedProperty initProp = property.FindPropertyRelative("Initialized");

        if (!initProp.boolValue)
        {
            SerializedProperty volProp = property.FindPropertyRelative("Volume");
            volProp.floatValue = 1f;
            SerializedProperty pitchProp = property.FindPropertyRelative("Pitch");
            pitchProp.floatValue = 1f;
            initProp.boolValue = true;
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }
}
#endif