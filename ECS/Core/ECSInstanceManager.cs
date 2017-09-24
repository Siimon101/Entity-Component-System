using System;

namespace ECS.Core
{
    public class ECSInstanceManager
    {

        private ECSEntityManager m_entityManager;
        private ECSComponentManager m_componentManager;
        private ECSSystemManager m_systemManager;
        private ECSQueryManager m_queryManager;


        public ECSInstanceManager(ECSQueryManager queryManager, ECSEntityManager entityManager, ECSComponentManager componentManager, ECSSystemManager systemManager)
        {
            m_queryManager = queryManager;
            m_entityManager = entityManager;
            m_componentManager = componentManager;
            m_systemManager = systemManager;
        }

        public Entity CreateEntity()
        {
            return m_entityManager.CreateEntity();
        }

        public void DestroyEntity(Entity e)
        {
            m_entityManager.DestroyEntity(e);
        }

        public T CreateSystem<T>() where T : ECSSystem
        {
            return m_systemManager.CreateSystem<T>(m_queryManager);
        }

        internal void DestroySystem<T>() where T : ECSSystem
        {
            m_systemManager.DestroySystem<T>();
        }

        public void DestroySystem(ECSSystem system)
        {
            m_systemManager.DestroySystem(system);
        }

        public T CreateComponent<T>() where T : ECSComponent
        {
            return CreateComponent<T>();
        }

        public T CreateComponent<T>(Entity e) where T : ECSComponent
        {
            return CreateComponent<T>(e.QueryID);
        }

        internal void AddComponent(Entity e, ECSComponent component)
        {
            m_componentManager.AddComponent(e.QueryID, component, false);
        }

        
        internal void AddComponent(Entity e, ECSComponent component, bool replace)
        {
            throw new NotImplementedException();
            m_componentManager.AddComponent(e.QueryID, component, replace);
        }

        public T CreateComponent<T>(int entityID) where T : ECSComponent
        {
            return m_componentManager.CreateComponent<T>(entityID);
        }

        public void DestroyComponent<T>(int entityID)
        {
            m_componentManager.DestroyComponent<T>(entityID);
        }

        public void DestroyComponent(ECSComponent component)
        {
            m_componentManager.DestroyComponent(component);
        }
    }
}