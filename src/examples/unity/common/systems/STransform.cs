using btcp.ECS.core;
using btcp.ECS.examples.unity.common.components;
using UnityEngine;

namespace btcp.ECS.examples.unity.common.systems
{
    public class STransform : ECSSystem
    {

        internal override void Update()
        {
            int[] entities = GetEntitiesWithComponents(typeof(CTransform));

            foreach (int eID in entities)
            {
                CTransform cTransform = GetComponent<CTransform>(eID);

                Vector3 pos = cTransform.GameObject.transform.position;
                pos.x = cTransform.X;
                pos.y = cTransform.Y;
                pos.z = cTransform.Z;
                cTransform.GameObject.transform.position = pos;
            }
        }

    }
}