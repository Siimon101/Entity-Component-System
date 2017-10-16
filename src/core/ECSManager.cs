using System;
using btcp.ECS.interfaces;

namespace btcp.ECS.core
{
    public class ECSManager
    {

        private ECSEntityManager m_entityManager;
        private ECSComponentManager m_componentManager;
        private ECSSystemManager m_systemManager;
        private ECSQueryManager m_queryManager;


        public ECSQueryManager QueryManager { get { return m_queryManager; } }
        public ECSComponentManager ComponentManager { get { return m_componentManager; } }
        public ECSEntityManager EntityManager { get { return m_entityManager; } }
        public ECSSystemManager SystemManager { get { return m_systemManager; } }


        public ECSManager()
        {
            m_entityManager = new ECSEntityManager();
            m_componentManager = new ECSComponentManager();
            m_systemManager = new ECSSystemManager();

            m_queryManager = new ECSQueryManager(m_componentManager, m_entityManager);

            m_systemManager.Initialize(m_queryManager, m_entityManager, m_componentManager);
        }

        public ECSManager(IECSEntityFactory entityFactory, IECSComponentFactory componentFactory) :this()
        {
            m_entityManager.Provide(entityFactory, m_componentManager);
            m_componentManager.Provide(componentFactory);
        }

        internal void Update()
        {
            m_systemManager.Update();
        }

        internal void FixedUpdate()
        {
            m_systemManager.FixedUpdate();
        }

        internal void LateUpdate()
        {
            m_systemManager.LateUpdate();
        }

        internal void Kill()
        {
            m_entityManager.Kill();
            m_systemManager.Kill();
        }
    }
}