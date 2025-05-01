using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Follow : MonoBehaviour
{
    public Transform[] waypoints;
    int curWaypoint;
    [Tooltip("Position speed")] [SerializeField] public float speed;
    [Tooltip("Rotation speed")][SerializeField] float rotSpeed;
    [Tooltip("Scalar speed")][SerializeField] float scaleSpeed;
    [Tooltip("Once the object has gone through all waypoints, it will not go to the beginning")] [SerializeField] bool dontLoop;
    [Tooltip("Positionally locks the Y position, meaning it will not go up or down")] [SerializeField] bool lockY;
    [Tooltip("Looks towards the next waypoint instead of copying its rotation")] [SerializeField] bool look;
    [Tooltip("Ignores Time.timeScale")] [SerializeField] bool ignoreTimescale;
    [Tooltip("Only called if Loop is set to false")] [SerializeField] UnityEvent OnComplete;


    Animator anim;
    Vector3 startPos;
    Quaternion startRot;
    Vector3 startScale;

    Vector3 targetPos;
    Quaternion targetRot;
    Vector3 targetScale;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        Transform waypoint = waypoints[curWaypoint].transform;
        Set(waypoint.position, waypoint.rotation, waypoint.localScale);
    }

    void OnDisable()
    {

    }

    private void Set(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        startPos = transform.position;
        startRot = transform.rotation;
        startScale = transform.localScale;
        targetPos = pos;
        targetRot = rot;
        targetScale = scale;
    }

    public void ChangeTarget(Transform parent)
    {
        curWaypoint = 0;
        List<Transform> tsf = new List<Transform>();
        foreach (Transform t in parent)
        {
            tsf.Add(t);
        }
        waypoints = tsf.ToArray();
    }

    private float Deltatime
    {
        get
        {
            return ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
        }
    }

    public Vector3 MoveTowards(Vector3 start, Vector3 cur, Vector3 end, float speed)
    {
        return Vector3.MoveTowards(cur, end, speed * Deltatime);
    }

    public Quaternion RotateTowards(Quaternion start, Quaternion cur, Quaternion end, float speed)
    {
        return Quaternion.RotateTowards(cur, end, speed * Deltatime);
    }

    // Update is called once per frame
    void Update()
    {
        Transform waypoint = waypoints[curWaypoint].transform;
        Set(waypoint.position, waypoint.rotation, waypoint.localScale);

        if (lockY) targetPos.y = transform.position.y;

        float dist = 0f;
        if (speed > 0f) dist = Vector3.Distance(targetPos, transform.position);
        else if (rotSpeed > 0f) dist = Quaternion.Angle(targetRot, transform.rotation);
        else if (scaleSpeed > 0f) dist = Vector3.Distance(targetScale, transform.localScale);
        if (dist < 0.01f)
        {
            curWaypoint++;


            if (curWaypoint > waypoints.Length - 1)
            {
                if (dontLoop)
                {
                    curWaypoint = 0;
                    enabled = false; OnComplete?.Invoke();
                }
                else curWaypoint = 0;
            }

            Transform waypoint = waypoints[curWaypoint].transform;
            Set(waypoint.position, waypoint.rotation, waypoint.localScale);
        }
            
        Quaternion rot = targetRot;
        if (look)
        {
            rot = Quaternion.LookRotation(targetPos - transform.position);
        } else transform.rotation = RotateTowards(startRot, transform.rotation, rot, rotSpeed);

        transform.position = MoveTowards(startPos, transform.position, targetPos, speed);
        if (scaleSpeed != 0f)
            transform.localScale = MoveTowards(startScale, transform.localScale, targetScale, scaleSpeed);
    }
}