using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Core
{
    public class ECSManager
    {

        private ECSSystemManager m_systemManager;
        private ECSEntityManager m_entityManager;
        private ECSComponentManager m_componentManager;
        private ECSQueryManager m_queryManager;
        private ECSInstanceManager m_instanceManager;

        internal ECSQueryManager QueryManager { get { return m_queryManager; } }
        internal ECSInstanceManager InstanceManager { get { return m_instanceManager; } }

        public ECSManager()
        {
            m_componentManager = new ECSComponentManager();
            m_systemManager = new ECSSystemManager();
            m_entityManager = new ECSEntityManager();

            m_queryManager = new ECSQueryManager(m_entityManager, m_componentManager, m_systemManager);
            m_instanceManager = new ECSInstanceManager(m_entityManager, m_componentManager, m_systemManager);

            m_systemManager.Initialize(m_queryManager);
        }


        public void Update()
        {
            m_systemManager.Update();
        }

        public void LateUpdate()
        {
            m_systemManager.LateUpdate();
        }

        public void FixedUpdate()
        {
            m_systemManager.FixedUpdate();
        }

    }
}