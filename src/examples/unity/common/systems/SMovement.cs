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

                cRigidbody.RigidBody.AddForce(cMovement.VelocityX, cMovement.VelocityY, cMovement.VelocityZ, ForceMode.VelocityChange);
            }
        }

    }
}