using System;

namespace com.btcp.ECS.core
{
    public class ECSManager
    {

        private ECSEntityManager m_entityManager;
        private ECSComponentManager m_componentManager;
        private ECSSystemManager m_systemManager;
        private ECSQueryManager m_queryManager;

        public ECSEntityManager EntityManager { get { return m_entityManager; } }
        public ECSComponentManager ComponentManager { get { return m_componentManager; } }
        public ECSSystemManager SystemManager { get { return m_systemManager; } }

        public ECSQueryManager QueryManager { get { return m_queryManager; } }


        public ECSManager()
        {
            m_entityManager = new ECSEntityManager();
            m_componentManager = new ECSComponentManager();
            m_systemManager = new ECSSystemManager();
            m_queryManager = new ECSQueryManager(m_componentManager, m_entityManager);

            m_systemManager.Initialize(m_queryManager);
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
    }
}