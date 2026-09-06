using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentState : MonoBehaviour
{
    [ColorUsageAttribute(false, true)] public Color skyColor;
    [ColorUsageAttribute(false, true)] public Color equatorColor;
    [ColorUsageAttribute(false, true)] public Color groundColor;
    public bool fog;
    public Color fogColor;
    public FogMode fogMode;
    public float fogDensity;
    public float transitionTime;
    public Material skybox;

    private void OnEnable()
    {
       if (transitionTime > 0f)
       {
            StartCoroutine(TransitionSettings());
       }
       else SetSettings();
    }

    private IEnumerator TransitionSettings()
    {
        Color ogEquator = RenderSettings.ambientEquatorColor;
        Color ogGround = RenderSettings.ambientGroundColor;
        Color ogSky = RenderSettings.ambientSkyColor;
        bool ogFog = RenderSettings.fog;
        Color ogFogColor = RenderSettings.fogColor;
        float ogFogDensity = RenderSettings.fogDensity;

        if (!ogFog && fog)
        {
            ogFogColor = Color.clear;
            ogFogDensity = 0f;
            RenderSettings.fog = true;
        }
                              
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * (1f / transitionTime);

            RenderSettings.ambientEquatorColor = Color.Lerp(ogEquator, equatorColor, t);
            RenderSettings.ambientGroundColor = Color.Lerp(ogGround, groundColor, t);
            RenderSettings.ambientSkyColor = Color.Lerp(ogSky, skyColor, t);
            RenderSettings.fogDensity = Mathf.Lerp(ogFogDensity, fogDensity, t);
            RenderSettings.fogColor = Color.Lerp(ogFogColor, fogColor, t);

            yield return null;
		}
		if (skybox)
			RenderSettings.skybox = skybox;
	}

    public void SetSettings()
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientEquatorColor = equatorColor;
        RenderSettings.ambientGroundColor = groundColor;
        RenderSettings.ambientSkyColor = skyColor;
        RenderSettings.fog = fog;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogMode = fogMode;
        RenderSettings.fogColor = fogColor;
        if (skybox)
            RenderSettings.skybox = skybox;
    }

    public void GetSettings()
    {
        equatorColor = RenderSettings.ambientEquatorColor;
        groundColor = RenderSettings.ambientGroundColor;
        skyColor = RenderSettings.ambientSkyColor;
        fog = RenderSettings.fog;
        fogColor = RenderSettings.fogColor;
        fogMode = RenderSettings.fogMode;
        fogDensity = RenderSettings.fogDensity;
        if (RenderSettings.skybox)
            skybox = RenderSettings.skybox;
    }
}
