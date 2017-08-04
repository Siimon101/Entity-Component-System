using System.Collections.Generic;
using Simon.Event_System;
using Simon.ECS.Manager;
using UnityEngine;
using IdleSiege.Actors.Entities.Combat.Components;
using IdleSiege.Actors.Units.Components;

namespace Simon.ECS
{
    public sealed class Entity : IECSQuerable
    {
        private int m_id;
        public int GetID()
        {
            return m_id;
        }

        private static int s_nextValidID = 0;

        private List<ECSComponent> m_components;

        private GameObject m_gameObject;
        public GameObject GameObject { get { return m_gameObject; } }

        public Entity(GameObject gameObject)
        {
            m_gameObject = gameObject;
            Init();
            ECSContainerManager.Add(this);
            EventDispatcher.s_Instance.SendEvent(new GameEvent(GameEvents.EVENT_ENTITY_SPAWNED, m_id));
            EventDispatcher.s_Instance.SendEvent(new GameEvent(GameEvents.EVENT_ENTITY_SPAWNED_LATE, m_id), .1f);
        }

        public void DestroyEntity()
        {
            UnityEngine.GameObject.Destroy(m_gameObject);
            ECSContainerManager.Remove(this);
        }


        private void Init()
        {
            HandleID();
            m_components = new List<ECSComponent>();
        }

        private void HandleID()
        {
            m_id = s_nextValidID;
            s_nextValidID++;
        }


        public void AddComponent(ECSComponent component)
        {
            ECSDebug.Log("Entity " + GetID() + " :: added component " + FormatComponentID(component));

            if (HasComponent(component.GetType()))
            {
                ECSDebug.LogError("Entity :: Tried adding component that already exists " + FormatComponentID(component));
                return;
            }

            m_components.Add(component);
            OnComponentAdded();
        }

        public bool HasComponent<T>()
        {
            return HasComponent(typeof(T));
        }

        public bool HasComponent(System.Type type)
        {
            return HasComponent(FormatComponentID(type));
        }

        private bool HasComponent(string id)
        {
            bool hasComponent = false;

            foreach (ECSComponent component in m_components)
            {
                if (FormatComponentID(component) == id)
                {
                    hasComponent = true;
                    break;
                }
            }

            return hasComponent;
        }

        public T GetComponent<T>() where T : ECSComponent
        {
            return (T)GetComponent(FormatComponentID<T>());
        }


        private ECSComponent GetComponent(string id)
        {
            if (HasComponent(id) == false)
            {
                ECSDebug.LogWarning("Entity " + GetID() + " :: Tried getting component that is not added");
            }

            ECSComponent componentReturned = null;
            foreach (ECSComponent component in m_components)
            {
                if (FormatComponentID(component) == id)
                {
                    componentReturned = component;
                    break;
                }
            }

            return componentReturned;
        }

        public void RemoveComponent<T>()
        {
            string componentType = FormatComponentID<T>();
            RemoveComponent(componentType);
        }

        public void RemoveComponent(ECSComponent component)
        {
            string componentType = FormatComponentID(component);
            RemoveComponent(componentType);
        }

        private void RemoveComponent(string id)
        {
            if (HasComponent(id))
            {
                ECSComponent component = GetComponent(id);
                m_components.Remove(component);
                OnComponentRemoved();
            }
        }

        public void RemoveAllComponents()
        {
            m_components = new List<ECSComponent>();
            OnComponentRemoved();
        }

        private string FormatComponentID(ECSComponent component)
        {
            return FormatComponentID(component.GetType());
        }

        private string FormatComponentID(System.Type type)
        {
            return type.Name;
        }


        private string FormatComponentID<T>()
        {
            return FormatComponentID(typeof(T));
        }


        private void OnComponentAdded()
        {
            GameEvent evt = new GameEvent(GameEvents.EVENT_ENTITY_COMPONENT_ADDED, GetID());
            EventDispatcher.s_Instance.SendEvent(evt);
            OnComponentChanged();
        }

        private void OnComponentRemoved()
        {
            GameEvent evt = new GameEvent(GameEvents.EVENT_ENTITY_COMPONENT_REMOVED, GetID());
            EventDispatcher.s_Instance.SendEvent(evt);
            OnComponentChanged();
        }

        private void OnComponentChanged()
        {
            GameEvent evt = new GameEvent(GameEvents.EVENT_ENTITY_COMPONENT_CHANGED, GetID());
            EventDispatcher.s_Instance.SendEvent(evt);
        }

    }
}
