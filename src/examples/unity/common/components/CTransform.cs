using btcp.ECS.core;
using UnityEngine;

namespace btcp.ECS.examples.unity.common.components
{
    public class CTransform : ECSComponent
    {
        public float X = float.NaN;
        public float Y = float.NaN;
        public float Z = float.NaN;
        public float RotationX = float.NaN;
        public float RotationY = float.NaN;
        public float RotationZ = float.NaN;

        public float ScaleX = float.NaN;
        public float ScaleY = float.NaN;
        public float ScaleZ = float.NaN;

        public string Name;
        public GameObject GameObject;

        public int LayerID = -1;

        ///The path of the prefab (must be located in Resources folder)
        public string PrefabID;

        public Vector3 Position3 { get { return new Vector3(X, Y, Z); } }
        public Vector3 Rotation3 { get { return new Vector3(RotationX, RotationY, RotationZ); } }
        public Vector3 Scale3 { get { return new Vector3(ScaleX, ScaleY, ScaleZ); } }

        public CTransform()
        {

        }

        public CTransform(GameObject gameObject)
        {
            this.GameObject = gameObject;
        }

        public CTransform(string name, float x, float y, float z) : this(x, y, z)
        {
            Name = name;
        }

        public CTransform(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public CTransform(float x, float y, float z, float rotX, float rotY, float rotZ) : this(x, y, z)
        {
            RotationX = rotX;
            RotationY = rotY;
            RotationZ = rotZ;
        }

        public CTransform(string name)
        {
            this.Name = name;
        }
    }
}