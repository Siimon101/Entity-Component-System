using System;
using btcp.ECS.core;
using btcp.ECS.examples.unity.common.components;
using UnityEngine;

namespace btcp.ECS.examples.unity.common.systems
{
    public class SMovement : ECSSystem
    {

        internal override void LateUpdate()
        {
            int[] entities = GetEntitiesWithComponents(typeof(CRigidbody), typeof(CMovement));

            foreach (int eID in entities)
            {
                CRigidbody cRigidbody = GetComponent<CRigidbody>(eID);
                CMovement cMovement = GetComponent<CMovement>(eID);

                Vector3 velocityDifference = cRigidbody.RigidBody.velocity - new Vector3(cMovement.VelocityX, cMovement.VelocityY, cMovement.VelocityZ);

                if (Vector3.Distance(cRigidbody.RigidBody.velocity, new Vector3(cMovement.VelocityX, cMovement.VelocityY, cMovement.VelocityZ)) == 0)
                {
                    continue;
                }

                float xSpeed = GetSpeed(cMovement.VelocityX, velocityDifference.x);
                float ySpeed = GetSpeed(cMovement.VelocityY, velocityDifference.y);
                float zSpeed = GetSpeed(cMovement.VelocityZ, velocityDifference.z);

                cRigidbody.RigidBody.AddForce(xSpeed, ySpeed, zSpeed, ForceMode.VelocityChange);
            }
        }

        private float GetSpeed(float targetSpeed, float speedDifference)
        {
            if (targetSpeed > Mathf.Abs(speedDifference))
            {
                return Mathf.Abs(speedDifference);
            }
            else
            {
                return targetSpeed;
            }
        }
    }
}