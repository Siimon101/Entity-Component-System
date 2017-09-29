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
            m_entityFactory = new ECSEntityFactory();
        }

        ///<summary> Adds <see cref="Entity"/> to ECS </summary>
        public Entity AddEntity(Entity entity)
        {
            m_entityBag.Add(entity);
            entity.EntityID = m_entityBag.GetSize();

            Message msg = new Message((int)MessageID.EVENT_ECS_ENTITY_CREATED);
            msg.SetArgInt("entity_id", entity.EntityID);
            MessageDispatcher.Instance.QueueMessage(msg);

            return entity;
        }

        ///<summary> Creates and adds <see cref="Entity"/> to ECS </summary>
        internal Entity CreateEntity()
        {
            Entity e = m_entityFactory.CreateEntity();
            return AddEntity(e);
        }

        ///<summary> Creates and adds <see cref="Entity"/> to ECS </summary>
        internal Entity CreateEntity(string archetype)
        {
            return m_entityFactory.CreateEntity(archetype);
        }

        public Entity GetEntity(int entityID)
        {
            return m_entityBag.Get(entityID);
        }

        public void DestroyEntity(int entityID)
        {
            Message msg = new Message((int)MessageID.EVENT_ECS_ENTITY_DESTROYED);
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