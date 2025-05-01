using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioPreset.asset", menuName = "ScriptableObjects/Audio Preset", order = 0)]
public class AudioPreset : ScriptableObject
{
    [Range(0f, 0.5f)]
    public float VolumeVariance;
    [Range(0f, 0.5f)]
    public float PitchVariance;
    public bool Spatial;
    [Range(0f, 360f)]
    public float Spread;
    [Range(0f, 1f)]
    public float Doppler;
    public float MinDistance;
    public float MaxDistance;
}