using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public float spawnVelocity;
    public bool randomize = true;
    public bool instantiate = true;
    private int index;
    public UnityEvent<GameObject> OnSpawn = new UnityEvent<GameObject>();

    private void OnEnable()
    {
        GameObject prefab = null;
        if (!randomize)
        {
            prefab = prefabs[index];
            index++;
            if (index == prefabs.Length)
                index = 0;
        }
        else prefab = prefabs[Random.Range(0, prefabs.Length)];

        if (prefab == null)
            return;

        GameObject inst = null;
        if (prefab.scene == SceneManager.GetActiveScene() && !instantiate)
        {
            prefab.SetActive(true);
            inst = prefab;
        }
        else
        {
            inst = GameObject.Instantiate(prefab, transform.position, transform.rotation);
            inst.name = prefab.name;
            inst.SetActive(true);
        }

        if (spawnVelocity != 0f)
        {
            inst.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * spawnVelocity;
        }
        OnSpawn.Invoke(inst);
        //enabled = false;
    }
}
