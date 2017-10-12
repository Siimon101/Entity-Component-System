using btcp.ECS.core;
using btcp.ECS.examples.unity.common.components;
using UnityEngine;

namespace btcp.ECS.examples.unity.common.systems
{
    public class STransform : ECSSystem
    {
        
        internal override void LateUpdate()
        {
            int[] entities = GetEntitiesWithComponents(typeof(CTransform));

            foreach (int eID in entities)
            {
                CTransform cTransform = GetComponent<CTransform>(eID);

                Vector3 pos = Vector3.zero;
                pos.x = cTransform.X;
                pos.y = cTransform.Y;
                pos.z = cTransform.Z;

                Vector3 rot = Vector3.zero;
                rot.x = cTransform.RotationX;
                rot.y = cTransform.RotationY;
                rot.z = cTransform.RotationZ;

                cTransform.GameObject.transform.position = (pos);
                cTransform.GameObject.transform.eulerAngles = (rot);
            }
        }

    }
}