using btcp.ECS.core;
using UnityEngine;

namespace btcp.ECS.examples.unity.common.components
{
    public class CMeshCollider : ECSComponent
    {
        public MeshCollider Collider;
        public bool IsConvex = false;

    }
}