using System;
using btcp.ECS.core;
using btcp.ECS.interfaces;
using btcp.ECS.utils;

namespace btcp.ECS.etc
{
    public class ECSEntityFactory_NULL : IECSEntityFactory
    {

        public Entity CreateEntity(string archetype)
        {
            return CreateEntity();
        }

        public Entity CreateEntity()
        {
            ECSDebug.LogWarning("EntityFactory was not provided.. Using " + GetType().Name);
            return new Entity();
        }

    }
}