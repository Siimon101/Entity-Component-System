using System;
using System.Collections.Generic;
using btcp.ECS.etc;
using btcp.ECS.interfaces;
using btcp.ECS.utils;

namespace btcp.ECS.core
{
    public class ECSEntityManager
    {

        public delegate void EntityCallback(ECSEntity e);
        public event EntityCallback OnEntityCreated;
        public event EntityCallback OnEntityDestroyedPre;
        public event EntityCallback OnEntityDestroyedPost;

        private Bag<ECSEntity> m_entityBag;

        private IECSEntityFactory m_entityFactory;
        internal static readonly int NULL_ENTITY = -1;

        public ECSEntityManager()
        {
            m_entityBag = new Bag<ECSEntity>();
            m_entityFactory = new ECSEntityFactory_NULL();
        }

        public void Provide(IECSEntityFactory factory, ECSComponentManager componentManager)
        {
            factory.Initialize(componentManager);
            m_entityFactory = factory;
        }

        ///<summary> Adds <see cref="ECSEntity"/> to ECS </summary>
        public ECSEntity AddEntity(ECSEntity entity)
        {
            int entityID = m_entityBag.GetSize();
            m_entityBag.Set(entityID, entity);
            entity.EntityID = entityID;

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
            AddEntity(e);
            return e;
        }

        internal void Kill()
        {
            foreach(ECSEntity entity in m_entityBag.GetAll())
            {
                if(entity != null)
                {
                    DestroyEntity(entity.EntityID);
                }
            }
        }

        ///<summary> Creates and adds <see cref="ECSEntity"/> to ECS </summary>
        internal ECSEntity CreateEntity(string archetype)
        {
            ECSEntity e = CreateEntity();
            m_entityFactory.SetupEntity(e, archetype);
            return e;
        }

        public ECSEntity GetEntity(int entityID)
        {
            return m_entityBag.Get(entityID);
        }

        public void DestroyEntity(int entityID)
        {
            ECSEntity entity = m_entityBag.Get(entityID);

            if(entity == null)
            {
                ECSDebug.LogWarning("Tried to destroy entity " + entityID + " but is already destroyed");
                return;
            }

            if (OnEntityDestroyedPre != null)
            {
                OnEntityDestroyedPre(entity);
            }

            m_entityFactory.DestroyEntity(entity);
            m_entityBag.Set(entityID, null);

            if (OnEntityDestroyedPost != null)
            {
                OnEntityDestroyedPost(entity);
            }

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