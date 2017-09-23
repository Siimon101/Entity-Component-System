using System;
using System.Collections.Generic;

namespace ECS.Core
{
    public class ECSComponentManager
    {

        private Dictionary<string, ECSComponentContainer> m_componentContainers;

        public ECSComponentManager()
        {
            m_componentContainers = new Dictionary<string, ECSComponentContainer>();
        }

        private void AddComponent(ECSComponent component)
        {
            GetComponentContainer(component).AddComponent(component);
        }

        private void RemoveComponent(ECSComponent component)
        {
            GetComponentContainer(component).RemoveComponent(component);
        }


        internal T CreateComponent<T>(int entityID) where T : ECSComponent
        {
            T component = Activator.CreateInstance<T>();
            AddComponent(entityID, component);
            return component;
        }

        internal void AddComponent(int entityID, ECSComponent component)
        {
            component.QueryID = entityID;
            AddComponent(component);
        }

        internal void DestroyComponent(ECSComponent component)
        {
            RemoveComponent(component);
        }

        internal void DestroyComponent<T>(int entityID)
        {
            GetComponentContainer<T>().RemoveComponent(entityID);
        }

        internal ECSComponent GetComponent<T>(int entityID)
        {
            return GetComponentContainer<T>().GetComponent(entityID);
        }

        internal ECSComponent GetComponent(int entityID, Type type)
        {
            return GetComponentContainer(GetComponentID(type)).GetComponent(entityID);
        }

        internal bool HasComponent(int entityID, Type type)
        {
            return GetComponentContainer(type).HasComponent(entityID);
        }

        private ECSComponentContainer GetComponentContainer<T>()
        {
            return GetComponentContainer(GetComponentID<T>());
        }

        private ECSComponentContainer GetComponentContainer(Type type)
        {
            return GetComponentContainer(GetComponentID(type));
        }

        private ECSComponentContainer GetComponentContainer(ECSComponent component)
        {
            return GetComponentContainer(GetComponentID(component));
        }

        private ECSComponentContainer GetComponentContainer(string componentID)
        {
            if (m_componentContainers.ContainsKey(componentID) == false)
            {
                m_componentContainers.Add(componentID, new ECSComponentContainer(componentID));
            }

            return m_componentContainers[componentID]; ;
        }


        internal string GetComponentID(ECSComponent component)
        {
            return GetComponentID(component.GetType());
        }

        internal string GetComponentID<T>()
        {
            return GetComponentID(typeof(T));
        }

        internal string GetComponentID(Type type)
        {
            return type.ToString();
        }

    }

    internal class ECSComponentContainer
    {
        private Dictionary<int, ECSComponent> m_components;

        private string m_containerType;
        public ECSComponentContainer(string type)
        {
            m_components = new Dictionary<int, ECSComponent>();
            m_containerType = type;
        }

        internal void AddComponent(ECSComponent component)
        {
            m_components.Add(component.QueryID, component);
        }

        internal void RemoveComponent(ECSComponent component)
        {
            m_components.Remove(component.QueryID);
        }

        internal void RemoveComponent(int entityID)
        {
            ECSComponent component = GetComponent(entityID);
            RemoveComponent(component);
        }

        internal ECSComponent GetComponent(int entityID)
        {
            if (m_components.ContainsKey(entityID) == false)
            {
                ECSDebug.LogError("Entity " + entityID + " does not have component of type " + m_containerType);
                return null;
            }

            return m_components[entityID];
        }

        internal bool HasComponent(int entityID)
        {
            return m_components.ContainsKey(entityID);
        }
    }
}