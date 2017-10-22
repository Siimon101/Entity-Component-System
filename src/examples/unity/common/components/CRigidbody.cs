using btcp.ECS.core;
using UnityEngine;

namespace btcp.ECS.examples.unity.common.components
{
    public class CRigidbody : ECSComponent
    {
        public Rigidbody RigidBody;
        public bool UseGravity = false;
        public bool IsKinematic = false;

    }
}