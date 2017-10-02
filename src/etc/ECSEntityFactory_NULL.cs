using System;
using btcp.ECS.core;
using btcp.ECS.interfaces;
using btcp.ECS.utils;

namespace btcp.ECS.etc
{
    public class ECSEntityFactory_NULL : IECSEntityFactory
    {

        public ECSEntity SetupEntity(ECSEntity e, string archetype)
        {
            ECSDebug.LogWarning("EntityFactory was not provided.. Using " + GetType().Name);
            return e;
        }

        public ECSEntity CreateEntity()
        {
            ECSDebug.LogWarning("EntityFactory was not provided.. Using " + GetType().Name);
            return new ECSEntity();
        }

    }
}