using btcp.ECS.core;
using btcp.ECS.examples.unity.common.components;
using UnityEngine;

namespace btcp.ECS.examples.unity.common.systems
{
    public class SRigidbody : ECSSystem
    {


        internal override void Update()
        {
            int[] entities = GetEntitiesWithComponents(typeof(CRigidbody), typeof(CTransform));

            foreach (int eID in entities)
            {
                CRigidbody cRigidbody = GetComponent<CRigidbody>(eID);
                CTransform cTransform = GetComponent<CTransform>(eID);

                Vector3 position = cRigidbody.RigidBody.position;

                cTransform.X = position.x;
                cTransform.Y = position.y;
                cTransform.Z = position.z;

                Vector3 rot = (cRigidbody.RigidBody.rotation.eulerAngles);
                cTransform.RotationX = rot.x;
                cTransform.RotationY = rot.y;
                cTransform.RotationZ = rot.z;

                if (cRigidbody.RigidBody.useGravity != cRigidbody.UseGravity)
                {
                    cRigidbody.RigidBody.useGravity = cRigidbody.UseGravity;
                }
                
                if (cRigidbody.RigidBody.isKinematic != cRigidbody.IsKinematic)
                {
                    cRigidbody.RigidBody.isKinematic = cRigidbody.IsKinematic;
                }

            }
        }

    }

}