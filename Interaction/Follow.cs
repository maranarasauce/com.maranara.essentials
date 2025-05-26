using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Maranara.Utility
{
    public class Follow : MonoBehaviour
    {
        public Transform waypointGroup;
        public Transform[] waypoints;
        int curWaypoint;
        [Tooltip("Position speed")][SerializeField] public float speed;
        [Tooltip("Rotation speed")][SerializeField] float rotSpeed;
        [Tooltip("Scalar speed")][SerializeField] float scaleSpeed;
        [Tooltip("Once the object has gone through all waypoints, it will not go to the beginning")][SerializeField] bool dontLoop;
        [Tooltip("Positionally locks the Y position, meaning it will not go up or down")][SerializeField] bool lockY;
        [Tooltip("Looks towards the next waypoint instead of copying its rotation")][SerializeField] bool look;
        [Tooltip("Ignores Time.timeScale")][SerializeField] bool ignoreTimescale;
        [Tooltip("Only called if Loop is set to false")][SerializeField] UnityEvent OnComplete;


        Animator anim;
        Vector3 startPos;
        Quaternion startRot;
        Vector3 startScale;

        Vector3 targetPos;
        Quaternion targetRot;
        Vector3 targetScale;
        void Start()
        {
            if (waypointGroup != null)
            {
                List<Transform> list = new List<Transform>();
                foreach (Transform t in waypointGroup)
                    list.Add(t);
                waypoints = list.ToArray();
            }

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

                Transform waypoint2 = waypoints[curWaypoint].transform;
                Set(waypoint2.position, waypoint2.rotation, waypoint2.localScale);
            }

            Quaternion rot = targetRot;
            if (look)
            {
                transform.rotation = RotateTowards(startRot, transform.rotation, Quaternion.LookRotation(targetPos - transform.position), rotSpeed);
            }
            else transform.rotation = RotateTowards(startRot, transform.rotation, rot, rotSpeed);

            transform.position = MoveTowards(startPos, transform.position, targetPos, speed);
            if (scaleSpeed != 0f)
                transform.localScale = MoveTowards(startScale, transform.localScale, targetScale, scaleSpeed);
        }

        private void OnDrawGizmos()
        {
            if (waypointGroup != null && waypoints.Length != waypointGroup.childCount)
            {
                List<Transform> list = new List<Transform>();
                foreach (Transform t in waypointGroup)
                    list.Add(t);
                waypoints = list.ToArray();
            }

            for (int i = 0; i < waypoints.Length; i++)
            {
                Gizmos.color = Color.cyan;
                Transform pt1 = null;
                if (i == 0)
                    pt1 = transform;
                else
                    pt1 = waypoints[i - 1];
                Transform pt2 = waypoints[i];

                Gizmos.DrawLine(pt1.position, pt2.position);
                GeneratePoint(pt1);

                if (i == waypoints.Length - 1)
                {
                    if (!dontLoop)
                        Gizmos.DrawLine(pt2.position, waypoints[0].position);
                    else Gizmos.color = Color.red;
                    GeneratePoint(pt2);
                }
            }
        }

        private void GeneratePoint(Transform tsf)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(tsf.position, tsf.position + tsf.forward);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(tsf.position, 0.1f);
        }
    }
}