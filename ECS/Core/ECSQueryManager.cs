using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities.MessageHandler;

namespace ECS.Core
{
    public class ECSQueryManager : IMessageListener
    {
        private ECSEntityManager m_entityManager;
        private ECSComponentManager m_componentManager;
        private ECSSystemManager m_systemManager;

        private List<ComponentQuery> m_cachedQueries;

        public int ListenerPriority { get { return 0; } }

        public ECSQueryManager(ECSEntityManager entityManager, ECSComponentManager componentManager, ECSSystemManager systemManager)
        {
            m_entityManager = entityManager;
            m_componentManager = componentManager;
            m_systemManager = systemManager;

            m_cachedQueries = new List<ComponentQuery>();

            MessageDispatcher.Instance.BindListener(this, (int)MessageID.EVENT_COMPONENT_ADDED);
            MessageDispatcher.Instance.BindListener(this, (int)MessageID.EVENT_COMPONENT_REMOVED);
            MessageDispatcher.Instance.BindListener(this, (int)MessageID.EVENT_ENTITY_CREATED);
            MessageDispatcher.Instance.BindListener(this, (int)MessageID.EVENT_ENTITY_DESTROYED);
        }


        internal Entity GetEntity(int id)
        {
            return m_entityManager.GetEntity(id);
        }

        internal T GetComponent<T>(Entity e) where T : ECSComponent
        {
            return (T)m_componentManager.GetEntityComponent<T>(e.QueryID);
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
            ComponentQuery query = FindQuery(args);
            return query.GetResult(m_entityManager);
        }


        private ComponentQuery FindQuery(params Type[] args)
        {
            ComponentQuery query = null;

            foreach (ComponentQuery q in m_cachedQueries)
            {
                if (q.HasTypes(args))
                {
                    return q;
                }
            }

            if (query == null)
            {
                string[] types = new string[args.Length];


                for (int i = 0; i < args.Length; i++)
                {
                    types[i] = args[i].ToString();
                }

                query = new ComponentQuery(types, m_componentManager);
                m_cachedQueries.Add(query);
            }


            return query;
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

        public void ReceiveMessage(Message message)
        {

            if (message.MessageID == (int)MessageID.EVENT_COMPONENT_ADDED || message.MessageID == (int)MessageID.EVENT_COMPONENT_REMOVED)
            {
                Entity entity = m_entityManager.GetEntity(message.GetArgInt("entity_id"));
                Type componentType = Type.GetType(message.GetArgString("component_type"));

                foreach (ComponentQuery query in m_cachedQueries)
                {
                    query.OnValueChanged(message.MessageID, entity, componentType);
                }
            }

            if (message.MessageID == (int)MessageID.EVENT_ENTITY_CREATED || message.MessageID == (int)MessageID.EVENT_ENTITY_DESTROYED)
            {
                Entity entity = m_entityManager.GetEntity(message.GetArgInt("entity_id"));

                foreach (ComponentQuery query in m_cachedQueries)
                {
                    query.OnValueChanged(message.MessageID, entity, null);
                }
            }

        }

        private class ComponentQuery
        {
            private string[] m_componentTypes;
            private List<Entity> m_cachedEntities;

            private HashSet<int> m_cachedReference;

            private bool m_isDirty = true;
            public bool IsDirty { get { return m_isDirty; } set { m_isDirty = value; } }

            private ECSComponentManager m_componentManager;

            public ComponentQuery(string[] types, ECSComponentManager componentManager)
            {
                m_componentManager = componentManager;
                m_componentTypes = types;
                m_isDirty = true;

                m_cachedReference = new HashSet<int>();
                m_cachedEntities = new List<Entity>();
            }


            public List<Entity> GetResult(ECSEntityManager entityManager)
            {
                if (m_isDirty)
                {
                    m_isDirty = false;
                    FetchEntities(entityManager);
                }

                return m_cachedEntities;
            }


            private void FetchEntities(ECSEntityManager entityManager)
            {
                m_cachedEntities.Clear();
                m_cachedReference.Clear();


                List<Entity> allEntities = entityManager.GetAll();

                for (int i = 0; i < allEntities.Count; i++)
                {
                    if (IsEntityValid(allEntities[i]))
                    {
                        CacheEntity(allEntities[i]);
                    }
                }

            }

            private void CacheEntity(Entity entity)
            {
                m_cachedEntities.Add(entity);
                m_cachedReference.Add(entity.QueryID);
            }

            private void DecacheEntity(Entity entity)
            {
                m_cachedReference.Remove(entity.QueryID);
                m_cachedEntities.Remove(entity);
            }

            private bool IsEntityValid(Entity entity)
            {
                Dictionary<Type, ECSComponent> cachedComponents = null;
                bool isValid = true;

                foreach (string type in m_componentTypes)
                {
                    isValid = true;
                    cachedComponents = m_componentManager.GetEntityComponents(entity.QueryID);

                    if (cachedComponents.ContainsKey(Type.GetType(type)) == false)
                    {
                        isValid = false;
                        break;
                    }
                }

                return isValid;
            }

            internal void OnValueChanged(int messageID, Entity entity, Type componentType)
            {

                if (messageID == (int)MessageID.EVENT_ENTITY_CREATED || messageID == (int)MessageID.EVENT_COMPONENT_ADDED)
                {
                    if (m_cachedReference.Contains(entity.QueryID) == false)
                    {
                        if (IsEntityValid(entity))
                        {
                            CacheEntity(entity);
                        }
                    }
                }

                if (messageID == (int)MessageID.EVENT_ENTITY_DESTROYED)
                {
                    if (m_cachedReference.Contains(entity.QueryID))
                    {
                        DecacheEntity(entity);
                        return;
                    }
                }

                if (messageID == (int)MessageID.EVENT_COMPONENT_REMOVED)
                {
                    if (componentType != null)
                    {
                        foreach (string type in m_componentTypes)
                        {
                            if (type.Equals(componentType) == false)
                            {
                                if (IsEntityValid(entity) == false)
                                {
                                    DecacheEntity(entity);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            internal bool HasTypes(Type[] args)
            {
                if (args.Length != m_componentTypes.Length)
                {
                    return false;
                }


                bool hasType = false;

                for (int i = 0; i < args.Length; i++)
                {
                    hasType = false;

                    for (int j = 0; j < m_componentTypes.Length; j++)
                    {
                        if (m_componentTypes[j].ToString().Equals(args[i].ToString()))
                        {
                            hasType = true;
                            break;
                        }
                    }

                    if (hasType == false)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}