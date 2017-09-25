using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Core
{
    public class ECSComponentManager
    {

        private Dictionary<Type, ECSComponentContainer> m_componentContainers;

        public ECSComponentManager()
        {
            m_componentContainers = new Dictionary<Type, ECSComponentContainer>();
        }


        private void RemoveComponent<T>(int entityID)
        {
            GetComponentContainer<T>().RemoveComponent(entityID);
        }

        internal void AddComponent(int entityID, ECSComponent component)
        {
            GetComponentContainer(component).AddComponent(entityID, component);
        }

        internal T CreateComponent<T>() where T : ECSComponent
        {
            ECSComponentContainer container = GetComponentContainer<T>();
            ECSComponent component = container.GetComponent();
            return (T)component;
        }

        internal T CreateComponent<T>(int entityID) where T : ECSComponent
        {
            T component = CreateComponent<T>();
            GetComponentContainer<T>().AddComponent(entityID, component);
            return (T)component;
        }

        internal void DestroyComponent<T>(int entityID)
        {
            GetComponentContainer<T>().RemoveComponent(entityID);
        }

        internal ECSComponent GetComponent<T>(int entityID)
        {
            return GetComponentContainer<T>().GetComponent(entityID);
        }


        internal bool HasComponent(int entityID, Type componentID)
        {
            return GetComponentContainer(componentID).HasComponent(entityID);
        }

        private ECSComponentContainer GetComponentContainer<T>()
        {
            return GetComponentContainer(GetComponentID(typeof(T)));
        }

        private ECSComponentContainer GetComponentContainer(ECSComponent component)
        {
            return GetComponentContainer(GetComponentID(component));
        }

        private ECSComponentContainer GetComponentContainer(Type componentID)
        {
            if (m_componentContainers.ContainsKey(componentID) == false)
            {
                m_componentContainers.Add(componentID, new ECSComponentContainer(componentID));
            }

            return m_componentContainers[componentID]; ;
        }


        internal Type GetComponentID(ECSComponent component)
        {
            return GetComponentID(component.GetType());
        }

        internal Type GetComponentID(Type type)
        {
            return type;
        }

    }

    internal class ECSComponentContainer
    {
        private List<ECSComponent> m_componentPool;
        private Dictionary<int, ECSComponent> m_components;

        private Type m_containerType;
        public ECSComponentContainer(Type type)
        {
            m_componentPool = new List<ECSComponent>();
            m_components = new Dictionary<int, ECSComponent>();
            m_containerType = type;
        }

        internal void AddComponent(int entityID, ECSComponent component)
        {
            m_components.Add(entityID, component);
        }

        internal void PoolComponent(ECSComponent eCSComponent)
        {
            m_componentPool.Add(eCSComponent);
            Log(eCSComponent, "Put Component in Pool.");
        }

        private void Log(ECSComponent component, string val)
        {
            ECSDebug.Log("[" + component.GetType().Name + "] Put Component in Pool.");
        }

        internal void RemoveComponent(int entityID)
        {
            ECSComponent removed = m_components[entityID];
            PoolComponent(removed);
            m_components.Remove(entityID);
        }

        internal ECSComponent GetComponent()
        {
            if (m_componentPool.Count == 0)
            {
                CreateComponent();
            }

            ECSComponent component = m_componentPool[0];
            m_componentPool.RemoveAt(0);

            Log(component, "Got Component from Pool");

            return component;
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

        internal void CreateComponent()
        {
            ECSComponent component = Activator.CreateInstance(m_containerType) as ECSComponent;
            PoolComponent(component);
        }
    }
}