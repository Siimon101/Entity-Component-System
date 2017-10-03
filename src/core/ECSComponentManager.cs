using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities.MessageHandler;
using btcp.ECS.etc;
using btcp.ECS.interfaces;
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
        private Dictionary<int, Bag<ECSComponent>> m_pendingComponents;
        private IECSComponentFactory m_componentFactory;

        public ECSComponentManager()
        {
            m_componentIdentifiers = new List<Type>();
            m_entityComponents = new Dictionary<int, Bag<ECSComponent>>();
            m_pendingComponents = new Dictionary<int, Bag<ECSComponent>>();
            m_componentFactory = new ECSComponentFactory_NULL();
        }

        public void Provide(IECSComponentFactory factory)
        {
            m_componentFactory = factory;
        }

        public ECSComponent CreateComponent<T>()
        {
            return m_componentFactory.CreateComponent<T>();
        }

        internal void CreateComponent<T>(int entityID)
        {
            ECSComponent component = CreateComponent<T>();
            AddComponent(entityID, component);
        }

        public void AddComponents(int entityID, Bag<ECSComponent> components)
        {
            foreach (ECSComponent component in components)
            {
                AddComponent(entityID, component);
            }
        }

        public ECSComponent AddComponent(int entityID, ECSComponent component)
        {
            AddPendingComponent(entityID, component);
            InitializePendingComponents(entityID);
            return null;
        }

        private void AddPendingComponent(int entityID, ECSComponent component)
        {
            SafeGetPendingComponentBag(entityID).Set(SafeGetComponentID(component.GetType()), component);
        }

        private void RemovePendingComponent(int entityID, ECSComponent component)
        {
            SafeGetPendingComponentBag(entityID).Set(GetComponentID(component.GetType()), null);
        }

        public void InitializePendingComponents(int entityID)
        {
            Bag<ECSComponent> bag = SafeGetPendingComponentBag(entityID);
            bag.ResizeToFit();
            List<ECSComponent> toInit = new List<ECSComponent>(bag.GetAll());
            ECSComponent component = null;

            int attemptThreshold = 10;


            while (toInit.Count > 0 && (toInit.Count > 1 && attemptThreshold > 0))
            {
                for (int i = toInit.Count - 1; i >= 0; i--)
                {
                    component = toInit[i];

                    if (m_componentFactory.InitializeComponent(entityID, component) == 0)
                    {
                        ECSDebug.Log("Initialized Component " + component);
                        SafeGetComponentBag(entityID).Set(SafeGetComponentID(component.GetType()), component);
                        OnComponentAdded(entityID, component);
                        toInit.RemoveAt(i);
                        continue;
                    }

                    attemptThreshold--;
                }
            }


            bag.Reset(toInit.Count);
            bag.Add(toInit);

            ECSDebug.Assert(attemptThreshold > 0, " Reached attempt threshold, maybe two components are dependent on eachother?");
        }



        private Bag<ECSComponent> SafeGetPendingComponentBag(int entityID)
        {
            if (m_pendingComponents.ContainsKey(entityID) == false)
            {
                m_pendingComponents.Add(entityID, new Bag<ECSComponent>());
            }

            return GetPendingComponentBag(entityID);
        }

        private Bag<ECSComponent> GetPendingComponentBag(int entityID)
        {
            return m_pendingComponents[entityID];
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

        public ECSComponent[] GetComponents(ECSEntity entity)
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



        internal void RemoveComponent<T>(int entityID)
        {
            ECSComponent component = GetComponentBag(entityID).Get(GetComponentID(typeof(T)));
            RemoveComponent(entityID, component);
        }

        public void RemoveComponent(int entityID, ECSComponent component)
        {
            OnComponentRemoved(entityID, component);
            m_componentFactory.DeInitializeComponent(entityID, component);
            GetComponentBag(entityID).Set(GetComponentID(component.GetType()), null);
        }

        private void RemoveAllComponents(int entityID)
        {
            Bag<ECSComponent> componentBag = GetComponentBag(entityID);
            componentBag.ResizeToFit();

            foreach (ECSComponent component in componentBag.GetAll())
            {
                RemoveComponent(entityID, component);
            }

            componentBag.Clear();
            m_entityComponents.Remove(entityID);
        }


        internal bool HasComponent<T>(int entityID)
        {
            return HasComponent(entityID, typeof(T));
        }


        public bool HasComponent(int entityID, Type type)
        {
            if (GetComponentID(type) == -1)
            {
                ECSDebug.LogWarning("[HasComponent " + type.Name + "] Component not yet registered " + type.Name.ToString());
                return false;
            }

            if (HasComponentBag(entityID) == false)
            {
                return false;
            }

            Bag<ECSComponent> bag = GetComponentBag(entityID);

            if (bag.Get(GetComponentID(type)) == null)
            {
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