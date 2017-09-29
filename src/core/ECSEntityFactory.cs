using System;
using btcp.ECS.etc;
using btcp.ECS.utils;
using UnityEngine;

namespace btcp.ECS.core
{
    public class ECSEntityFactory
    {
        
        private IECSEntityCreator m_entityCreator;
        public ECSEntityFactory()
        {
            m_entityCreator = new NULLEntityCreator();
        }

        public void Provide(IECSEntityCreator entityCreator)
        {
            m_entityCreator = entityCreator;
        }

        internal Entity CreateEntity()
        {
            return new Entity();
        }

        public Entity CreateEntity(string archetype)
        {
            ECSDebug.Assert(m_entityCreator != null, "Entity Creator not provided!");

            Entity entity = CreateEntity();
            m_entityCreator.CreateEntityFromArchetype(entity, archetype);
            return entity;
        }
    }
}