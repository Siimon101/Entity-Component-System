using btcp.ECS.core;
using UnityEngine;

namespace btcp.ECS.examples.unity.common.components
{
    public class CSpriteRenderer : ECSComponent
    {
        public SpriteRenderer SpriteRenderer;
        public string SpriteID;

        public CSpriteRenderer()
        {
            
        }
        public CSpriteRenderer(SpriteRenderer renderer)
        {
            SpriteRenderer = renderer;
        }

        public CSpriteRenderer(SpriteRenderer renderer, string spriteID)
        {
            SpriteRenderer = renderer;
            SpriteID = spriteID;
        }
    }
}