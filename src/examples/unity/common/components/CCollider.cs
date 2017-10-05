using System.Collections.Generic;
using btcp.ECS.core;
using UnityEngine;

namespace btcp.ECS.examples.unity.common.components
{
    public class CCollider : ECSComponent
    {
        public Collider Collider;
        public List<int> Collisions;


        public CCollider()
        {
            Collisions = new List<int>();
        }
    }
}