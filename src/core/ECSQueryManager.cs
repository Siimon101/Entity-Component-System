using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities.MessageHandler;
using btcp.ECS.core;
using btcp.ECS.examples.unity.common.components;
using btcp.ECS.utils;

namespace btcp.ECS.core
{
    public class ECSQueryManager
    {
        //TODO : Consider creating ImmutableArray (non modifiable) for Bag elements, instead of calling bag.GetAll().Clone()
        private Dictionary<Type[], Bag<int>> m_queries;
        private ECSComponentManager m_componentManager;
        private ECSEntityManager m_entityManager;


        public ECSQueryManager(ECSComponentManager componentManager, ECSEntityManager entityManager)
        {
            m_queries = new Dictionary<Type[], Bag<int>>();
            m_componentManager = componentManager;
            m_entityManager = entityManager;

            m_componentManager.OnComponentAdded += OnComponentAdded;
            m_componentManager.OnComponentRemoved += OnComponentAdded;
            m_entityManager.OnEntityDestroyed += OnEntityDestroyed;
        }

        private Bag<int> GetQuery(Type[] args)
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
                    return m_queries[typeArray];
            }

            return null;
        }

        internal bool SafeHasComponent<T>(int entityID) where T : ECSComponent
        {
            return (m_componentManager.SafeGetComponent<T>(entityID) != null);
        }

        private int[] CreateQuery(Type[] args)
        {
            ECSDebug.Log("Created query with args " + args.Length);
            m_queries[args] = new Bag<int>();
            RefreshQuery(args);
            return m_queries[args].GetAll().Clone() as int[];
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


            bag.Reset(validIdentifiers.Count);
            bag.Add(validIdentifiers);
            bag.ResizeToFit();

        }

        private void OnEntityDestroyed(ECSEntity entity)
        {
            int entityID = entity.EntityID;
            Bag<int> query = null;

            foreach (Type[] type in m_queries.Keys)
            {
                query = GetQuery(type);

                if (query.Has(entityID) > -1)
                {
                    query.RemoveIndex(query.Has(entityID));
                    query.ResizeToFit();
                }
            }
        }

        private void OnComponentAdded(int entityID, ECSComponent component)
        {
            Bag<int> query = null;

            foreach (Type[] type in m_queries.Keys)
            {
                query = GetQuery(type);

                if (query.Has(entityID) == -1)
                {
                    if (m_componentManager.HasComponents(entityID, type))
                    {
                        query.Add(entityID);
                        query.ResizeToFit();
                    }
                }
            }
        }


        private void OnComponentRemoved(int entityID, ECSComponent component)
        {
            Bag<int> query = null;

            foreach (Type[] type in m_queries.Keys)
            {
                query = GetQuery(type);

                if (m_componentManager.HasComponents(entityID, type) == false)
                {
                    query.RemoveIndex(query.Has(entityID));
                    query.ResizeToFit();
                }
            }
        }




        internal T GetComponent<T>(int entityID) where T : ECSComponent
        {
            return m_componentManager.GetComponent<T>(entityID);
        }

        internal T SafeGetComponent<T>(int entityID) where T : ECSComponent
        {
            if (m_componentManager.HasComponent<T>(entityID) == false)
            {
                return null;
            }

            return m_componentManager.GetComponent<T>(entityID);
        }

        public int[] GetEntitiesWithComponents(params Type[] args)
        {
            Bag<int> cachedQuery = GetQuery(args);

            if (cachedQuery != null)
            {
                return cachedQuery.GetAll().Clone() as int[];
            }

            return CreateQuery(args);
        }

        internal bool IsEntityValid(int entityID)
        {
            return m_entityManager.IsEntityValid(entityID);
        }



    }
}