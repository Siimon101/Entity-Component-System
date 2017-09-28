using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities.MessageHandler;
using btcp.ECS.utils;

namespace btcp.ECS.core
{
    public class ECSEntityManager
    {
        private Bag<Entity> m_entityBag;

        private ECSEntityFactory m_entityFactory;

        public ECSEntityManager()
        {
            m_entityBag = new Bag<Entity>();
        }

        public void Initialize(ECSComponentManager componentManager)
        {
            m_entityFactory = new ECSEntityFactory(componentManager);
        }


        public Entity AddEntity(Entity entity)
        {
            m_entityBag.Add(entity);
            entity.EntityID = m_entityBag.GetSize();

            Message msg = new Message((int)MessageID.EVENT_ENTITY_ON_CREATED);
            msg.SetArgInt("entity_id", entity.EntityID);
            MessageDispatcher.Instance.QueueMessage(msg);

            return entity;
        }

        internal Entity CreateEntity()
        {
            Entity e = new Entity();
            return AddEntity(e);
        }

        internal Entity CreateEntity(string archetype)
        {
            Entity entity = CreateEntity();
            m_entityFactory.LoadArchetype(entity.EntityID, archetype);
            return entity;
        }

        public Entity GetEntity(int entityID)
        {
            return m_entityBag.Get(entityID);
        }

        public void DestroyEntity(int entityID)
        {
            Message msg = new Message((int)MessageID.EVENT_ENTITY_ON_DESTROYED);
            msg.SetArgInt("entity_id", entityID);
            MessageDispatcher.Instance.QueueMessage(msg);
            
            m_entityBag.Set(entityID, null);
        }

        public int[] GetEntityIdentifiers()
        {
            List<Entity> validEntities = m_entityBag.GetValid();
            int[] validEntityIdentifiers = new int[validEntities.Count];

            for (int i = 0; i < validEntities.Count; i++)
            {
                validEntityIdentifiers[i] = validEntities[i].EntityID;
            }

            return validEntityIdentifiers;
        }

        internal bool IsEntityValid(int entityID)
        {
            return (m_entityBag.Get(entityID) != null);
        }
    }
}