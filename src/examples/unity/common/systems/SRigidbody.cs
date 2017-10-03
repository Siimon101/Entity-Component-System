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

                cTransform.X = cRigidbody.RigidBody.position.x;
                cTransform.Y = cRigidbody.RigidBody.position.y;
                cTransform.Z = cRigidbody.RigidBody.position.z;
            }
        }

    }

}