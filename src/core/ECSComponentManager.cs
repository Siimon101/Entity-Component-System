using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities.MessageHandler;
using btcp.ECS.utils;

namespace btcp.ECS.core
{
    public class ECSComponentManager
    {

        private List<Type> m_componentIdentifiers;
        private Dictionary<int, Bag<ECSComponent>> m_entityComponents;

        public ECSComponentManager()
        {
            m_componentIdentifiers = new List<Type>();
            m_entityComponents = new Dictionary<int, Bag<ECSComponent>>();
        }


        public Entity AddComponent(Entity entity, ECSComponent component)
        {
            SafeGetComponentBag(entity.EntityID).Set(SafeGetComponentID(component.GetType()), component);

            Message msg = new Message((int)MessageID.EVENT_ENTITY_COMPONENT_ADDED);
            msg.SetArgInt("entity_id", entity.EntityID);
            MessageDispatcher.Instance.QueueMessage(msg);

            return entity;
        }

        private Bag<ECSComponent> SafeGetComponentBag(int entityID)
        {
            if (m_entityComponents.ContainsKey(entityID) == false)
            {
                m_entityComponents.Add(entityID, new Bag<ECSComponent>());
            }

            return GetComponentBag(entityID);
        }

        private Bag<ECSComponent> GetComponentBag(int entityID)
        {
            return m_entityComponents[entityID];
        }

        private bool HasComponentBag(int entityID)
        {
            return m_entityComponents.ContainsKey(entityID);
        }

        public T SafeGetComponent<T>(int entityID) where T : ECSComponent
        {
            return SafeGetComponentBag(entityID).Get(SafeGetComponentID(typeof(T))) as T;
        }

        public T GetComponent<T>(int entityID) where T : ECSComponent
        {
            return GetComponentBag(entityID).Get(GetComponentID(typeof(T))) as T;
        }

        public ECSComponent[] GetComponents(Entity entity)
        {
            return SafeGetComponentBag(entity.EntityID).GetAll();
        }


        private int SafeGetComponentID(Type type)
        {
            if (m_componentIdentifiers.IndexOf(type) == -1)
            {
                m_componentIdentifiers.Add(type);
            }

            return GetComponentID(type);
        }

        private int GetComponentID(Type type)
        {
            return m_componentIdentifiers.IndexOf(type);
        }

        public Entity RemoveComponent<T>(Entity entity) where T : ECSComponent
        {
            SafeGetComponentBag(entity.EntityID).Set(SafeGetComponentID(typeof(T)), null);

            Message msg = new Message((int)MessageID.EVENT_ENTITY_COMPONENT_REMOVED);
            msg.SetArgInt("entity_id", entity.EntityID);
            MessageDispatcher.Instance.QueueMessage(msg);

            return entity;
        }

        internal bool HasComponents(int id, Type[] args)
        {
            if (HasComponentBag(id) == false)
            {
                ECSDebug.LogWarning("Entity " + id + " does not have any components!");
                return false;
            }

            Bag<ECSComponent> bag = GetComponentBag(id);

            foreach (Type type in args)
            {
                if (GetComponentID(type) == -1)
                {
                    ECSDebug.LogWarning("Component not yet registered " + type.Name.ToString());
                    return false;
                }

                if (bag.Get(GetComponentID(type)) == null)
                {
                    ECSDebug.LogWarning("Entity " + id + " does not have component " + type.Name.ToString() + " (Component ID : " + GetComponentID(type) + ")");
                    return false;
                }
            }

            return true;
        }
    }
}