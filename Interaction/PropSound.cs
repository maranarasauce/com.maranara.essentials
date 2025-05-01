using Maranara.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maranara
{
    public class PropSound : MonoBehaviour
    {
        public float volumeModifier = 1f;
        public float velocityFloor = 0.1f;
        public float velocityCeiling = 30f;
        public AudioInfo OnCollide;

        //Whether the audio is playing or not
        private bool audioPlaying = false;

        private float lengthAudio = 1f;     //The length of the audio file
        private float timeElapsed = 0f;     //The length of time that has elapsed

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

        }

        private void OnCollisionEnter(Collision collision)
        {
            //If we are not colliding with the player
            if (!collision.transform.CompareTag("Player"))
            {
                ContactPoint pt = collision.GetContact(0);
                float vel = collision.relativeVelocity.sqrMagnitude;

                if (vel >= velocityFloor)
                {
                    OnCollide.Volume = Mathf.InverseLerp(velocityFloor, velocityCeiling, vel) * volumeModifier;
                    PoolManager.PlayAudio(OnCollide, pt.point);
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            //If we are not colliding with the player
            if (!collision.transform.CompareTag("Player"))
            {
                //Get the contact point of the collision
                ContactPoint pt = collision.GetContact(0);

                //Get the velocity of the collision
                float vel = collision.relativeVelocity.sqrMagnitude;

                //If the velocity is greater than or equal to the floor velocity
                if (vel >= velocityFloor)
                {
                    //Set the volume of the OnCollide audio using the velocity and volume modifiers
                    OnCollide.Volume = Mathf.InverseLerp(velocityFloor, velocityCeiling, vel) * volumeModifier;

                    //If audio is not playing
                    if (!audioPlaying)
                    {
                        //Play the audio at the contact point
                        PoolManager.PlayAudio(OnCollide, pt.point);

                        //Audio is now playing
                        audioPlaying = true;

                        //Reset length of time
                        timeElapsed = 0f;
                    }
                    else
                    {
                        //Increase time elapsed by delta time
                        timeElapsed += Time.deltaTime;

                        //If time elapsed is greater than or equal to audio length
                        if (timeElapsed >= lengthAudio)
                            //Set audio playing to false
                            audioPlaying = false;
                    }
                }
                else
                {
                    //If the velocity drops then the audio file is no longer playing
                    audioPlaying = false;
                }
            }
        }
    }
}
