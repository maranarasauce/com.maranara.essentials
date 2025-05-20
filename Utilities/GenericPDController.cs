using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    public class GenericPDController : MonoBehaviour
    {
        void Start()
        {
            if (rb == null)
                rb = GetComponent<Rigidbody>();

            rbTransform = rb.transform;
        }

        void FixedUpdate()
        {
            AddForceTowardsTarget();
        }

        public float frequency = 30;
        public float damping = 1;
        public float forceMultiplier = 1;
        public float torqueMultiplier = 1;
        public Transform target;
        [NonSerialized] public Transform rbTransform;
        public Rigidbody rb;
        [NonSerialized] public Vector3 targetLastPos;

        //https://digitalopus.ca/site/pd-controllers/
        void AddForceTowardsTarget()
        {
            Vector3 Vdes = (targetLastPos - target.position);
            float kp = (6f * frequency) * (6f * frequency) * 0.25f;
            float kd = 4.5f * frequency * damping;
            float dt = Time.fixedDeltaTime;
            float g = 1 / (1 + kd * dt + kp * dt * dt);
            float ksg = kp * g;
            float kdg = (kd + kp * dt) * g;
            Vector3 Pt0 = rbTransform.position;
            Vector3 Vt0 = rb.linearVelocity;
            Vector3 Pdes = target.position;
            Vector3 F = (Pdes - Pt0) * ksg + (Vdes - Vt0) * kdg;
            rb.AddForce(F, ForceMode.Acceleration);
            targetLastPos = target.position;

            Quaternion desiredRotation = target.rotation;
            Vector3 x;
            float xMag;
            Quaternion q = desiredRotation * Quaternion.Inverse(rbTransform.rotation);
            // Q can be the-long-rotation-around-the-sphere eg. 350 degrees
            // We want the equivalant short rotation eg. -10 degrees
            // Check if rotation is greater than 190 degees == q.w is negative
            if (q.w < 0)
            {
                // Convert the quaterion to eqivalent "short way around" quaterion
                q.x = -q.x;
                q.y = -q.y;
                q.z = -q.z;
                q.w = -q.w;
            }
            q.ToAngleAxis(out xMag, out x);
            x.Normalize();
            x *= Mathf.Deg2Rad;
            Vector3 pidv = kp * x * xMag - kd * rb.angularVelocity;
            Quaternion rotInertia2World = rb.inertiaTensorRotation * rbTransform.rotation;
            pidv = Quaternion.Inverse(rotInertia2World) * pidv;
            pidv.Scale(rb.inertiaTensor);
            pidv = rotInertia2World * pidv;
            rb.AddTorque(pidv, ForceMode.Acceleration);
        }
    }
}