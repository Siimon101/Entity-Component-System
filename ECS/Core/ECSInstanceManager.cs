using System;
using Game.Systems;

namespace ECS.Core
{
    public class ECSInstanceManager
    {

        private ECSEntityManager m_entityManager;
        private ECSComponentManager m_componentManager;
        private ECSSystemManager m_systemManager;


        public ECSInstanceManager(ECSEntityManager entityManager, ECSComponentManager componentManager, ECSSystemManager systemManager)
        {
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

        internal void RegisterSystem(ECSSystem system, bool autoInitialize = true)
        {
            m_systemManager.RegisterSystem(system, autoInitialize);
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
            return m_componentManager.CreateComponent<T>();
        }

        public T CreateComponent<T>(Entity e) where T : ECSComponent
        {
            return CreateComponent<T>(e.QueryID);
        }

        internal void AddComponent(Entity e, ECSComponent component)
        {
            m_componentManager.AddComponent(e.QueryID, component);
        }

        public T CreateComponent<T>(int entityID) where T : ECSComponent
        {
            return m_componentManager.CreateComponent<T>(entityID);
        }

    }
}