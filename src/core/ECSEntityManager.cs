using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities.MessageHandler;
using btcp.ECS.etc;
using btcp.ECS.interfaces;
using btcp.ECS.utils;

namespace btcp.ECS.core
{
    public class ECSEntityManager
    {

        public delegate void EntityCallback(ECSEntity e);
        public event EntityCallback OnEntityCreated;
        public event EntityCallback OnEntityDestroyed;

        private Bag<ECSEntity> m_entityBag;

        private IECSEntityFactory m_entityFactory;

        public ECSEntityManager()
        {
            m_entityBag = new Bag<ECSEntity>();
            m_entityFactory = new ECSEntityFactory_NULL();
        }

        public void Provide(IECSEntityFactory factory)
        {
            m_entityFactory = factory;
        }

        ///<summary> Adds <see cref="ECSEntity"/> to ECS </summary>
        public ECSEntity AddEntity(ECSEntity entity)
        {
            m_entityBag.Add(entity);
            entity.EntityID = m_entityBag.GetSize();

            if (OnEntityCreated != null)
            {
                OnEntityCreated(entity);
            }

            return entity;
        }

        ///<summary> Creates and adds <see cref="ECSEntity"/> to ECS </summary>
        internal ECSEntity CreateEntity()
        {
            ECSEntity e = m_entityFactory.CreateEntity();
            return AddEntity(e);
        }

        ///<summary> Creates and adds <see cref="ECSEntity"/> to ECS </summary>
        internal ECSEntity CreateEntity(string archetype)
        {
            ECSEntity e = CreateEntity();
            m_entityFactory.SetupEntity(e, archetype);
            return AddEntity(e);
        }

        public ECSEntity GetEntity(int entityID)
        {
            return m_entityBag.Get(entityID);
        }

        public void DestroyEntity(int entityID)
        {
            if (OnEntityDestroyed != null)
            {
                OnEntityDestroyed(m_entityBag.Get(entityID));
            }

            m_entityBag.Set(entityID, null);
        }

        public int[] GetEntityIdentifiers()
        {
            List<ECSEntity> validEntities = m_entityBag.GetValid();
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