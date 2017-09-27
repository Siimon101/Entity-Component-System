using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities.MessageHandler;
using btcp.ECS.utils;

namespace btcp.ECS.core
{
    public class ECSEntityManager
    {

        private Bag<Entity> m_entityBag;


        public ECSEntityManager()
        {
            m_entityBag = new Bag<Entity>();
        }


        public Entity AddEntity(Entity entity)
        {
            entity.EntityID = m_entityBag.GetSize();
            m_entityBag.Add(entity);

            Message msg = new Message((int)MessageID.EVENT_ENTITY_ON_CREATED);
            msg.SetArgInt("entity_id", entity.EntityID);
            MessageDispatcher.Instance.QueueMessage(msg);

            return entity;
        }

        public Entity CreateEntity()
        {
            Entity entity = new Entity();
            return AddEntity(entity);
        }

        public Entity GetEntity(int entityID)
        {
            return m_entityBag.Get(entityID);
        }

        public void RemoveEntity(int entityID)
        {
            ECSDebug.Log("Remove Entity " + entityID);
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