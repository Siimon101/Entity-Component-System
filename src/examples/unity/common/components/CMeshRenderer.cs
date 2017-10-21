using btcp.ECS.core;
using UnityEngine;

namespace btcp.ECS.examples.unity.common.components
{
    public class CMeshRenderer : ECSComponent
    {

        public MeshRenderer MeshRenderer;
        public string MaterialID = null;

        public MeshFilter MeshFilter;
        public string MeshID = null;


    }
}