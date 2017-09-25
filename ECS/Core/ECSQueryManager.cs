using System;
using System.Collections.Generic;

namespace ECS.Core
{
    public class ECSQueryManager
    {
        private ECSEntityManager m_entityManager;
        private ECSComponentManager m_componentManager;
        private ECSSystemManager m_systemManager;

        public ECSQueryManager(ECSEntityManager entityManager, ECSComponentManager componentManager, ECSSystemManager systemManager)
        {
            m_entityManager = entityManager;
            m_componentManager = componentManager;
            m_systemManager = systemManager;
        }


        internal Entity GetEntity(int id)
        {
            return m_entityManager.GetEntity(id);
        }

        internal T GetComponent<T>(Entity e) where T : ECSComponent
        {
            return (T)m_componentManager.GetComponent<T>(e.QueryID);
        }

        internal ECSSystem GetSystem(string id)
        {
            return m_systemManager.GetSystem(id);
        }

        internal ECSSystem GetSystem<T>() where T : ECSSystem
        {
            return m_systemManager.GetSystem<T>();
        }

        internal List<Entity> GetEntitiesWithComponents(params System.Type[] args)
        {
            List<Entity> allEntities = m_entityManager.GetAll();
            List<Entity> entitiesReturned = new List<Entity>(allEntities);


            Dictionary<Type, ECSComponent> cachedComponents = null;
            bool toRemove = false;

            for (int i = entitiesReturned.Count - 1; i >= 0; i--)
            {
                toRemove = false;
                cachedComponents = m_componentManager.GetComponents(entitiesReturned[i].QueryID);


                foreach (Type type in args)
                {
                    if (cachedComponents.ContainsKey(type) == false)
                    {
                        toRemove = true;
                        break;
                    }
                }

                if (toRemove)
                {
                    entitiesReturned.RemoveAt(i);
                }
            }

            return entitiesReturned;
        }

        internal void StartSytem<T>() where T : ECSSystem
        {
            m_systemManager.StartSystem<T>();
        }

        internal void StopSystem<T>() where T : ECSSystem
        {
            m_systemManager.StopSystem<T>();
        }

        internal void ToggleSystem<T>() where T : ECSSystem
        {
            m_systemManager.ToggleSystem<T>();
        }
    }
}