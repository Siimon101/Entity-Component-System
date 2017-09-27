using System;
using System.Collections.Generic;
using com.btcp.ECS.utils;

namespace com.btcp.ECS.core
{
    public class ECSQueryManager
    {

        private Dictionary<Type[], Bag<int>> m_queries;
        private ECSComponentManager m_componentManager;
        private ECSEntityManager m_entityManager;

        public ECSQueryManager(ECSComponentManager componentManager, ECSEntityManager entityManager)
        {
            m_queries = new Dictionary<Type[], Bag<int>>();
            m_componentManager = componentManager;
            m_entityManager = entityManager;
        }

        public int[] GetEntitiesWithComponents(params Type[] args)
        {
            int[] cachedQuery = GetQuery(args);

            if (cachedQuery != null)
            {
                return cachedQuery;
            }

            return CreateQuery(args);
        }

        private int[] GetQuery(Type[] args)
        {
            bool hasType = false;

            foreach (Type[] typeArray in m_queries.Keys)
            {
                hasType = false;

                if (typeArray.Length != args.Length)
                {
                    continue;
                }

                foreach (Type typeB in args)
                {
                    hasType = false;

                    foreach (Type typeA in typeArray)
                    {
                        if (typeA.ToString().Equals(typeB.ToString()))
                        {
                            hasType = true;
                            break;
                        }
                    }

                    if (hasType == false)
                    {
                        break;
                    }
                }

                if (hasType)
                    return m_queries[typeArray].GetAll();
            }

            return null;
        }


        private int[] CreateQuery(Type[] args)
        {
            UnityEngine.Debug.Log("Created query for args " + args.Length);
            m_queries[args] = new Bag<int>();
            RefreshQuery(args);
            return m_queries[args].GetAll();
        }

        private void RefreshQuery(Type[] args)
        {
            Bag<int> bag = m_queries[args];

            int[] entityIdentifiers = m_entityManager.GetEntityIdentifiers();

            List<int> validIdentifiers = new List<int>();
            foreach (int id in entityIdentifiers)
            {
                if (m_componentManager.HasComponents(id, args))
                {
                    validIdentifiers.Add(id);
                }
            }

            bag.SetCapacity(validIdentifiers.Count);
            bag.Add(validIdentifiers);

        }

        internal T GetComponent<T>(int entityID) where T : ECSComponent
        {
            return m_componentManager.GetComponent<T>(entityID);
        }

    }
}