using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVelocity : MonoBehaviour
{
    public Vector3 vel;
    public bool relative;
	public bool onAwake = true;
    private Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();

		if (onAwake)
			Set(rb);
	}

	public void Set()
	{
		Set(rb);
	}

	public void Set(Rigidbody rigidbody)
	{
		Vector3 newVel = vel;
		if (relative)
			newVel = transform.rotation * newVel;
        rigidbody.velocity = newVel;
	}
}
