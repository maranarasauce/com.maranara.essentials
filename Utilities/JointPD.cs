using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara.Utility
{
    public class JointPD : MonoBehaviour
    {
        static Rigidbody parent;
        Rigidbody rb;
        ConfigurableJoint jt;
        [SerializeField] float damp;
        [SerializeField] float spring;
        [SerializeField] float maxForce;
        [SerializeField] bool scaleMass;
        [SerializeField] Transform targetTsf;
        Quaternion originalRot;

        private void Awake()
        {
            if (parent == null)
            {
                GameObject go = new GameObject();
                parent = go.AddComponent<Rigidbody>();
                parent.name = "PDParent";
                parent.isKinematic = true;
            }

            rb = GetComponent<Rigidbody>();

            jt = parent.gameObject.AddComponent<ConfigurableJoint>();
            JointDrive drive = new JointDrive()
            {
                maximumForce = maxForce,
                positionSpring = spring,
                positionDamper = damp
            };
            jt.yDrive = drive;
            jt.xDrive = drive;
            jt.zDrive = drive;
            jt.angularXDrive = drive;
            jt.angularYZDrive = drive;
            jt.connectedBody = rb;
            jt.autoConfigureConnectedAnchor = false;
            jt.connectedAnchor = jt.transform.position;
            jt.anchor = Vector3.zero;
            if (scaleMass)
                jt.connectedMassScale = rb.mass;

            originalRot = transform.rotation;
        }
        private void FixedUpdate()
        {
            jt.targetPosition = targetTsf.position;
            jt.SetTargetRotationLocal(Quaternion.Inverse(targetTsf.rotation), originalRot);
            //jt.anchor = pos;
        }
    }
}