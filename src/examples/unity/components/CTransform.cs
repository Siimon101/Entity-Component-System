using btcp.ECS.core;
using UnityEngine;

namespace btcp.ECS.examples.unity.components
{
    public class CTransform : ECSComponent
    {
        public float X;
        public float Y;
        public float Z;

        public string Name;
        public GameObject GameObject;

        public CTransform()
        {

        }

        public CTransform(GameObject gameObject, string name, float x, float y, float z)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
        }
    }
}