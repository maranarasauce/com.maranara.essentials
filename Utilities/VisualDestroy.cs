using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDestroy : MonoBehaviour
{
	public bool onEnable = true;
	public float scale = 1f;
	public float volume = 0f;

	public GameObject fx;
	public bool overrideMaterial = true;
	public Material material;
    public Transform point;

	private void Awake()
	{
		if (material == null)
		{
			foreach (var mr in gameObject.GetComponentsInChildren<Renderer>(true)){
				material = mr.sharedMaterial;
			}
		}
	}

	private void OnEnable()
	{
		if (onEnable)
			Destroy();
	}

	public void Destroy()
	{
		if (point == null)
		{
			point = transform;
		}

		var go = GameObject.Instantiate(fx, point.position, Quaternion.identity);
		if (overrideMaterial && material != null && go.TryGetComponent(out ParticleSystemRenderer psr))
		{
			psr.material = material;
		}
		if ((volume > 0f || scale != 1f) && go.TryGetComponent(out ParticleSystem ps))
		{
			var shape = ps.shape;
			shape.radius = volume;
			var main = ps.main;
			main.startSizeMultiplier *= scale;
		}
		GameObject.Destroy(gameObject);
	}
}
