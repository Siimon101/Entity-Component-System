using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Core
{
    public class ECSComponentManager
    {

        private Dictionary<string, ECSComponentContainer> m_componentContainers;

        public ECSComponentManager()
        {
            m_componentContainers = new Dictionary<string, ECSComponentContainer>();
        }

        internal void AddComponent(int entityID, ECSComponent component, bool replace)
        {

            if (HasComponent(entityID, GetComponentID(component)) == true)
            {
                if (replace)
                {
                    OverwriteComponent(entityID, component);
                    return;
                }

                ECSDebug.LogWarning("Tried adding component to entity " + entityID + " but component with same type exists (" + GetComponentID(component) + ")");
                return;
            }

            GetComponentContainer(component).AddComponent(entityID, component);
        }

        internal void OverwriteComponent(int entityID, ECSComponent component)
        {
            if (HasComponent(entityID, GetComponentID(component)) == false)
            {
                ECSDebug.LogError("Tried to overwrite component but entity " + entityID + " does not have component " + GetComponentID(component));
                return;
            }

            throw new NotImplementedException();
        }

        private void RemoveComponent(ECSComponent component)
        {
            GetComponentContainer(component).RemoveComponent(component);
        }


        internal T CreateComponent<T>() where T : ECSComponent
        {
            T component = Activator.CreateInstance<T>();
            GetComponentContainer(component).PoolComponent(component);
            return component;
        }

        internal T CreateComponent<T>(int entityID) where T : ECSComponent
        {
            T component = CreateComponent<T>();
            AddComponent(entityID, component, false);
            return component;
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

        internal ECSComponent GetComponent(int entityID, string componentID)
        {
            return GetComponentContainer(componentID).GetComponent(entityID);
        }

        internal bool HasComponent(int entityID, string componentID)
        {
            return GetComponentContainer(componentID).HasComponent(entityID);
        }

        private ECSComponentContainer GetComponentContainer<T>()
        {
            return GetComponentContainer(GetComponentID<T>());
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

        internal void AddComponent(int entityID, ECSComponent component)
        {
            component.QueryID = entityID;
            m_components.Add(component.QueryID, component);
        }

        internal void PoolComponent(ECSComponent eCSComponent)
        {
            ECSDebug.LogWarning("Tried to pool " + eCSComponent.GetType() + " but pooling not yet implemented.");
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
            ECSDebug.Log(m_containerType + " - " + entityID);

            return m_components.ContainsKey(entityID);
        }
    }
}