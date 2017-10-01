using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities.MessageHandler;
using btcp.ECS.utils;

namespace btcp.ECS.core
{
    public class ECSComponentManager
    {

        public delegate void ComponentCallback(int entityID, ECSComponent component);
        public event ComponentCallback OnComponentAdded;
        public event ComponentCallback OnComponentRemoved;

        private List<Type> m_componentIdentifiers;
        private Dictionary<int, Bag<ECSComponent>> m_entityComponents;

        public ECSComponentManager()
        {
            m_componentIdentifiers = new List<Type>();
            m_entityComponents = new Dictionary<int, Bag<ECSComponent>>();
        }

        public ECSComponent AddComponent(Entity entity, ECSComponent component)
        {
            return AddComponent(entity.EntityID, component);
        }

        public ECSComponent AddComponent(int entityID, ECSComponent component)
        {
            SafeGetComponentBag(entityID).Set(SafeGetComponentID(component.GetType()), component);

            OnComponentAdded(entityID, component);
            return component;
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


        public int SafeGetComponentID(Type type)
        {
            if (m_componentIdentifiers.IndexOf(type) == -1)
            {
                m_componentIdentifiers.Add(type);
            }

            return GetComponentID(type);
        }

        public int GetComponentID(Type type)
        {
            return m_componentIdentifiers.IndexOf(type);
        }

        public Entity RemoveComponent<T>(Entity entity) where T : ECSComponent
        {
            ECSDebug.Assert(entity != null, "Entity does not exist");

            int componentID = GetComponentID(typeof(T));

            OnComponentRemoved(entity.EntityID,  GetComponentBag(entity.EntityID).Get(componentID));

            GetComponentBag(entity.EntityID).Set(componentID, null);

            return entity;
        }


        private void RemoveAllComponents(int v)
        {
            Bag<ECSComponent> componentBag = GetComponentBag(v);
            componentBag.Clear();
            m_entityComponents.Remove(v);
        }


        internal bool HasComponent<T>(int entityID)
        {
            return HasComponent(entityID, typeof(T));
        }


        public bool HasComponent(int entityID, Type type)
        {
            if (HasComponentBag(entityID) == false)
            {
                ECSDebug.LogWarning("Entity " + entityID + " does not have any components!");
                return false;
            }

            Bag<ECSComponent> bag = GetComponentBag(entityID);
            if (GetComponentID(type) == -1)
            {
                ECSDebug.LogWarning("Component not yet registered " + type.Name.ToString());
                return false;
            }

            if (bag.Get(GetComponentID(type)) == null)
            {
                ECSDebug.LogWarning("Entity " + entityID + " does not have component " + type.Name.ToString() + " (Component ID : " + GetComponentID(type) + ")");
                return false;
            }

            return true;
        }

        internal bool HasComponents(int id, Type[] args)
        {
            foreach (Type type in args)
            {
                if (HasComponent(id, type) == false)
                {
                    return false;
                }
            }

            return true;
        }


    }
}