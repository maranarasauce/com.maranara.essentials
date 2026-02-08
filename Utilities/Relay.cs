using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Relay : MonoBehaviour
{
	[Flags]
	public enum ActivationTrigger
	{
		None = 0,
		Enabled = 1,
		Disabled = 2,
		PlayerEnter = 4,
		PlayerExit = 8
	}

	public enum GameObjectType
	{
		Toggle,
		InvertOnDisable
	}

	[SerializeField] bool activateOnce = true;
	[SerializeField] float delay;
    [SerializeField] ActivationTrigger activationType = ActivationTrigger.Enabled;
    [SerializeField] GameObjectType objectEnabling;
	[Space(10)]
	[SerializeField] GameObject[] enabledObjects;
	[SerializeField] GameObject[] disabledObjects;
	[SerializeField] UnityEvent EnableEvent;
	[SerializeField] UnityEvent DisableEvent;

    private void OnEnable()
    {
        if ((activationType & ActivationTrigger.Enabled) != ActivationTrigger.None)
		{
			Activate();
		}
    }

    private void OnDisable()
    {
		if ((activationType & ActivationTrigger.Disabled) != ActivationTrigger.None)
		{
			Disactivate();
		}
	}

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    bool hasActivated;
    public void Activate()
	{
		if (activateOnce && hasActivated)
			return;
		hasActivated = true;
		Invoke(nameof(DoActivation), delay);
	}

	private void DoActivation()
	{
		EnableEvent?.Invoke();
		foreach (var obj in enabledObjects)
		{
			obj.SetActive(true);
		}
		foreach (var obj in disabledObjects)
		{
			obj.SetActive(false);
		}
	}

	bool hasDisactivated;
	public void Disactivate()
	{
		if (activateOnce && hasDisactivated)
			return;
		hasDisactivated = true;
		Invoke(nameof(DoDisactivation), delay);
	}

	private void DoDisactivation()
	{
		DisableEvent?.Invoke();
		if (objectEnabling == GameObjectType.InvertOnDisable)
		{
			foreach (var obj in enabledObjects)
			{
				obj.SetActive(false);
			}
			foreach (var obj in disabledObjects)
			{
				obj.SetActive(true);
			}
		}
	}
}
