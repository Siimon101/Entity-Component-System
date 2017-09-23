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

        internal T GetComponent<T>(int id) where T : ECSComponent
        {
            return (T)m_componentManager.GetComponent<T>(id);
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

            Entity entity = null;

            foreach (System.Type type in args)
            {
                for (int i = entitiesReturned.Count - 1; i >= 0; i--)
                {
                    entity = entitiesReturned[i];

                    if (m_componentManager.HasComponent(entity.QueryID, type) == false)
                    {
                        entitiesReturned.RemoveAt(i);
                    }
                }
            }

            return entitiesReturned;
        }

    }
}